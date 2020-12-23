const baseurl = "https://localhost:44319";
if(!Boolean(sessionStorage.getItem("jwt"))){
    window.location.href = "login_registration.html";
}
const jwt = sessionStorage.getItem("jwt");

window.addEventListener("load",(event) =>{
    function get_active_name(){
        debugger;
        var ret = "";
        if(window.location.search.includes("?")){
            var params = window.location.search.split('?')[1].split('&');
            let obj = new Object();
            for (let i of params){
                let aux = i.split('=');
                obj[aux[0]] = aux[1];
            }
            ret = obj.name;
        }
        return ret;
    };
    (async function () {
        //debugger;
        var params = {
            method : "GET",
            headers: {"Authorization":`Bearer ${jwt}`}
        };
        var response = await fetch(`${baseurl}/api/character`,params);
        try {
            if (response.status === 200){
                message = await response.json();
                debugger;
                let name_active = get_active_name();
                let lista = message.map((c) =>
                {
                    let elem =`<a onclick="fetch_phrases(${c.id},'${c.name}');" class="list-group-item list-group-item-action ${name_active==c.name?"active":""}" id="list-home-list" data-toggle="list" href="#list-home" role="tab" aria-controls="home" aria-selected="false">${c.name.toUpperCase()}</a>`
                    if(name_active===c.name){
                        fetch_phrases(c.id,c.name);
                    }
                    active = false;
                    return elem;
                });
                document.getElementById('list-tab').innerHTML= lista.join(' ');
               
            }
        }
        catch(error){

        }
    })();
});

async function fetch_phrases(idchar,name) {
    //debugger;
    document.getElementById("component-aux").innerHTML = `${idchar}-${name}`;
    fillTitle();

    var response = await fetch(`${baseurl}/api/character/${idchar}/phrase`);
    try {
        if (response.status===200) {
            data = await response.json();
            let frases = data.map((p)=>{
                // let elem = `
                // <div class="card" style="width: 300px; margin-bottom:30px; border-radius:15px;">
                // <img class="card-img-top logo" src="./imgs/logo-${name}.png" alt="Card image ${name}" style="width:70%; height:200px; margin: 5% 15%"> 
                // `;
                let elem = `
                            <div style = "background-color:white; padding:10px; border-radius:10px; list-style-type: none;">
                            <li>${p.content.toUpperCase()}
                            <div style="display:flex; justify-content:flex-end;">
                                <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#Modal${p.id}"> Update </button>
                                <button id="deletebtn-${idchar}" type="button" class="btn btn-danger" data-toggle="modal" onclick="fetch_delete(${p.id});"> Delete </button>
                            </div>
                            </li>
                            </div><br>
                            <div class="modal fade" id="Modal${p.id}" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                                <div class="modal-dialog" role="document">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                        <h5 class="modal-title" id="exampleModalLabel">Update Phrase</h5>
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                            <span aria-hidden="true">X</span>
                                        </button>
                                        </div>
                                        <div class="modal-body">
                                        <form id="updatefrm-${p.id}">
                                            <div class="form-group">
                                            <label for="content${p.id}">Content</label>
                                            <input type="text" class="form-control" id="content${p.id}" value="${p.content}" placeholder="${p.content}" name="content">
                                            </div>
                                            <div class="form-group">
                                            <label for="popularity${p.id}">Popularity</label>
                                            <select class="form-control" id="popularity${p.id}" name="popularity" placeholder="${p.popularity}">
                                                <option>high</option>
                                                <option>medium</option>
                                                <option>low</option>
                                            </select>
                                            </div>
                                            <div class="form-group">
                                            <label for="season${p.id}">Appearing Season</label>
                                            <input type="number" class="form-control" id="season${p.id}" placeholder="${p.season}" name="season">
                                            </div>
                                        </form>
                                        </div>
                                        <div class="modal-footer">
                                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                                        <button id="updatebtn-${p.id}" type="button" class="btn btn-primary" onclick = "fetch_update(${idchar});">Save changes</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                           
                            `;
                return elem;
            });
            document.getElementById('list-phrases').innerHTML=`<img class="logo" style="border-radius:10px; height:300px; width: 25%; margin-left:33%; margin-bottom: 30px;" src="../imgs/logo-${name}.png"><ul>${frases.join(' ')}</ul>`;
        }
    } catch (error) {
        
    }
}
async function fetch_delete(idphrase){
    //debugger;
    const baseurl = "https://localhost:44319";
    const event = window.event;
    event.preventDefault();
    var id = event.target.id;
    id = parseInt(id.split('-')[1]);

    var params = { method: "DELETE"};
    var response = await fetch(`${baseurl}/api/character/${id}/phrase/${idphrase}`,params);
    try {
        if(response.status == 200){
            var json = await response.json();
            if(Boolean(json)){
                alert('The phrase was deleted.');
            }
            else{
                alert('Something happend, the phrase was not deleted');
            }
            window.location.href = 'phrases.html';
        }
        else{
            throw new error(await response.text());
        }
    } catch (error) {
        console.error(error.message);
    }
}
function fillTitle(){   
    var name = document.getElementById("component-aux").innerHTML.split('-')[1];
    document.getElementById("create-title").innerHTML=`Create for ${name}`;
}
async function fetch_update(idchar){
    const baseurl = "https://localhost:44319";
    const event = window.event;
    event.preventDefault();
    var id = event.target.id;
    id = parseInt(id.split('-')[1]);
    debugger;
    var frm = document.getElementById(`updatefrm-${id}`);
    var data = {
        Content : frm.content.value.length != 0? frm.content.value:null,
        Season : parseInt(frm.season.value),
        Popularity : frm.popularity.value.length != 0? frm.popularity.value: null
    };
    var params = {
        method : "PUT",
        body : JSON.stringify(data),
        headers: { "Content-Type": "application/json; charset=utf-8" }
    };
    var response = await fetch(`${baseurl}/api/character/${parseInt(idchar)}/phrase/${id}`, params);
    try {
        if(response.status === 200){
            var json = await response.json();
            if(Boolean(json)){
                alert("The changes was applied.");
            }
            else{
                alert("Something happend, The changes could not be applied.");
            }
            window.location.href = "phrases.html";
        }
        else{
            if(response.status === 400){
                var json= await response.json();
                for(let key in json){
                    if (!(json[key][0].toLowerCase().includes("required")))
                    {
                        frm[`${key.toLowerCase()}`].labels[0].innerHTML += `<i style="color:red;"> ${json[key][0]}</i>`;
                        frm[`${key.toLowerCase()}`].value = "";
                    }
                }
                alert("Some fields were invalidated.")
            }
            else{
                throw new error (await response.text());
            }
        }
    } catch (error) {
        console.error(error.message);
    }
};
async function fetch_create(){
    const baseurl = "https://localhost:44319";
    const event = window.event;
    event.preventDefault();
    var frm = document.getElementById(`frm-create`);
    var data = {
        Content : frm.content.value.length != 0? frm.content.value:null,
        Season : parseInt(frm.season.value),
        Popularity : frm.popularity.value.length != 0? frm.popularity.value: null
    };
    var params = {
        method : "POST",
        body : JSON.stringify(data),
        headers: { "Content-Type": "application/json; charset=utf-8" }
    };
    debugger;
    var characterId = document.getElementById("component-aux").innerHTML.split('-')[0];
    var response = await fetch(`${baseurl}/api/character/${characterId}/phrase`, params);
    try {
        if(response.status === 201){
            var json = await response.json();
            if(Boolean(json)){
                alert("The Phrase was creeated successfully.");
            }
            else{
                alert("Something happend, The changes could not be applied.");
            }
            window.location.href = "phrases.html";
        }
        else{
            if(response.status === 400){
                
                var json= await response.json();
                for(let key in json){
                    frm[`${key.toLowerCase()}`].labels[0].innerHTML += `<i style="color:red;"> ${json[key][0]}</i>`;
                    frm[`${key.toLowerCase()}`].value = "";
                };
                alert("Some fields were invalidated.");
            }
            else{
                throw new error (await response.text());
            }
        }
    } catch (error) {
        console.error(error.message);
    }
};
