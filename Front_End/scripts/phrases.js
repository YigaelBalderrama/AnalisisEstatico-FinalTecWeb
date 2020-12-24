const baseurl = "https://localhost:44319";
// if(!Boolean(sessionStorage.getItem("jwt"))){
//     window.location.href = "login_registration.html";
// }
var jwt = "";//sessionStorage.getItem("jwt");
window.addEventListener("load",(event) =>{
    function logout(event){
        sessionStorage.clear();
        window.location.href = "phrases.html"
      }
    if(Boolean(sessionStorage.getItem("jwt"))){
        jwt=sessionStorage.getItem("jwt");
        debugger;
        document.getElementById("login-state").innerHTML = `
        <h7> You are Logged </h7> 
        <button id="button-logout" class="btn btn-light" style="background-color: rgb(255, 255, 255); color: black;">Logout</button>`;
        document.getElementById("button-logout").addEventListener('click',logout);
        var disabled_buttons = document.querySelectorAll("[disabled]");
        for(let elem of disabled_buttons){
            debugger;
            elem.disabled=false;
            elem.title = "";
        }
    }
    function get_active_name(){
        //debugger;
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
        debugger;
        var params = {
            method : "GET",
            headers: {"Authorization":`Bearer ${jwt}`}
        };
        var response = await fetch(`${baseurl}/api/character`,params);
        try {
            if (response.status === 200){
                message = await response.json();
                let name_active = get_active_name();
                let lista = message.map((c) =>
                {
                    let elem =`
                        <a onclick="fetch_phrases(${c.id},'${c.name}');" class="list-group-item list-group-item-action ${name_active==c.name?"active":""}" id="list-home-list" data-toggle="list" href="#list-home" role="tab" aria-controls="home" aria-selected="false">${c.name.toUpperCase()}</a>`;
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
//register phrases liked before leaving the page
window.onbeforeunload = function(){
    register_likes();
};
//show phrases of a selected character
async function fetch_phrases(idchar,name) {
    debugger;
    register_likes();
    document.getElementById("component-aux").innerHTML = `${idchar}-${name}`;
    fillTitle();
    var params = {headers:{"Authorization":`Bearer ${jwt}`}};
    var response = await fetch(`${baseurl}/api/character/${idchar}/phrase`,params);
    try {
        if (response.status===200) {
            data = await response.json();
            let frases = data.map((p)=>{
                let elem = `
                            <div style = "background-color:white; padding:10px; border-radius:10px; list-style-type: none;">
                            <li>${p.content.toUpperCase()}
                            <div class="like" style="display:inline;"> 
                                <button id="likebutton-${p.id}" class="btn btn-light" onclick="like_phrase('${p.id}');"> 
                                    <img id="likelogo-${p.id}" name="none" src="../imgs/logo-like.png" style="width: 20px;"></img> 
                                </button>
                                <i id= textlikes-${p.id}> ${p.likes} likes</i>
                            </div>
                            <div style="display:flex; justify-content:flex-end;">
                                <button ${(jwt==="")? `title = "need to login" disabled` :title=""} type="button" class="btn btn-primary restricted" data-toggle="modal" data-target="#Modal${p.id}" > Update </button>
                                <button ${(jwt==="")? `title = "need to login" disabled` :title=""} id="deletebtn-${idchar}" type="button" class="btn btn-danger" data-toggle="modal" onclick="fetch_delete(${p.id});" > Delete </button>
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
//change title of create collapse element and reload auxiliar variable 
function fillTitle(){   
    var name = document.getElementById("component-aux").innerHTML.split('-')[1];
    document.getElementById("create-title").innerHTML=`Create for ${name}`;
}
//register liked phrases before change to other character view
async function register_likes(){
    //debugger;
    var liked_phrases = Array.prototype.slice.call(document.querySelectorAll("img[name=liked]"));
    if(liked_phrases.length != 0){
        liked_phrases = liked_phrases.map((p) =>{
            return parseInt(p.id.split('-')[1]);
        });
        var params ={
            method: "PUT",
            body : JSON.stringify(liked_phrases),
            headers: { "Content-Type": "application/json; charset=utf-8","Authorization":`Bearer ${jwt}`}
        };
        var characterId = document.getElementById("component-aux").innerHTML.split('-')[0];
        var response = await fetch(`${baseurl}/api/character/${characterId}/phrase/like`,params);
        try {
            if(response.status == 200){
                var json = await response.json();
                if(Boolean(json)){
                    console.log('The likes was registered.');
                }
                else{
                    console.log('Something happend, the likes was not registered');
                }
            }
            else{
                throw new error(await response.text());
            }
        } catch (error) {
            console.error(error.message);
        }
    }
}
//show visible changes after push the like button
function like_phrase(phraseId){
    //debugger;
    var logo = document.getElementById(`likelogo-${phraseId}`);
    var button = document.getElementById(`likebutton-${phraseId}`);
    var texto = document.getElementById(`textlikes-${phraseId}`);
    if(logo.name === "none"){
        logo.src = "../imgs/logo-nonlike.png";
        logo.name = "liked";
        button.style.backgroundColor = "#a2e0ff";
        texto.textContent = `${parseInt(texto.textContent)+1} likes`;
    }
    else{
        if(logo.name === "liked"){
            logo.src = "../imgs/logo-like.png";
            logo.name = "none";
            button.style.backgroundColor = "";
            texto.textContent = `${parseInt(texto.textContent)-1} likes`;
        }
    }
};
//delete phrase
async function fetch_delete(idphrase){
    //debugger;
    const baseurl = "https://localhost:44319";
    const event = window.event;
    event.preventDefault();
    var id = event.target.id;
    id = parseInt(id.split('-')[1]);

    var params = { method: "DELETE", headers:{"Authorization":`Bearer ${jwt}`}};
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

async function fetch_update(idchar){
    const baseurl = "https://localhost:44319";
    const event = window.event;
    event.preventDefault();
    var id = event.target.id;
    id = parseInt(id.split('-')[1]);
    //debugger;
    var frm = document.getElementById(`updatefrm-${id}`);
    var data = {
        Content : frm.content.value.length != 0? frm.content.value:null,
        Season : parseInt(frm.season.value),
        Popularity : frm.popularity.value.length != 0? frm.popularity.value: null
    };
    var params = {
        method : "PUT",
        body : JSON.stringify(data),
        headers: { "Content-Type": "application/json; charset=utf-8", "Authorization":`Bearer ${jwt}` }
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
        headers: { "Content-Type": "application/json; charset=utf-8", "Authorization":`Bearer ${jwt}` }
    };
    //debugger;
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
