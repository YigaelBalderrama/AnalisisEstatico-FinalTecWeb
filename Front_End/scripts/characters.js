if(!Boolean(sessionStorage.getItem("jwt"))){
    window.location.href = "login_registration.html";
}
const jwt = sessionStorage.getItem("jwt");

window.addEventListener("load",(event) =>{
    const baseurl = "https://localhost:44319";
    var showCharacters = (async function (criteria = "none") {
        debugger;
        var params = {
            method : "GET",
            headers: {"Authorization":`Bearer ${jwt}`}
        };
        var response = await fetch(`${baseurl}/api/character`,params);
        try {
            if (response.status === 200){
                message = await response.json();
                if(criteria!= "none"){
                    if(criteria == "name"){
                        message = message.sort((o1,o2) => (o1.name>o2.name)?1:((o2.name>o1.name)?-1:0));
                    }
                    if(criteria == "age"){
                        message = message.sort((o1,o2) => (o1.age>o2.age)?1:((o2.age>o1.age)?-1:0));
                    }
                    if(criteria == "appearingSeason"){
                        message = message.sort((o1,o2) => (o1.appearingSeason>o2.appearingSeason)?1:((o2.appearingSeason>o1.appearingSeason)?-1:0));
                    }
                }
                lista = message.map((c) => {
                    let elem =   `<div class="card " style="width: 300px; margin-bottom:30px; border-radius:15px;">
                    <img class="card-img-top logo" src="./imgs/logo-${c.name}.png" alt="Card image ${c.name}" style="width:70%; height:200px; margin: 5% 15%">
                    <div class="card-body">
                    <h5>${c.name}</h5>  
                    <p class="card-text">
                      <b>Name</b>: ${c.name}<br>
                      <b>Age</b>: ${c.age}<br>
                      <b>Protagonist</b>: ${(c.isProta)?"Protagonista":"No Protagonista"}<br>
                      <b>Occupation</b>: ${c.occupation}<br>
                      <b>Appearing Season</b>: ${c.appearingSeason}<br><br>
                      <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#Modal${c.id}"> Update </button>
                      <button id="deletebtn-${c.id}" type="button" class="btn btn-danger" data-toggle="modal" onclick="fetch_delete();"> Delete </button>

                        <!-- Modal -->
                        <div class="modal fade" id="Modal${c.id}" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="exampleModalLabel">Update ${c.name}</h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body">
                            <form id="updatefrm-${c.id}">
                                <div class="form-group">
                                    <label for="name${c.id}">Name</label>
                                    <input type="text" class="form-control" id="name${c.id}" placeholder="${c.name}" name="name">
                                </div>
                                <div class="form-group">
                                    <label for="age${c.id}">Age</label>
                                    <input type="number" class="form-control" id="age${c.id}" placeholder="${c.age}" name="age">
                                </div>
                                <div class="form-group">
                                    <label for="occupation${c.id}">Occupation</label>
                                    <input type="text" class="form-control" id="occupation${c.id}" placeholder="${c.occupation}" name="occupation">
                                </div>
                                <div class="form-group">
                                    <label for="appearing${c.id}">Appearing Season</label>
                                    <input type="number" class="form-control" id="appearing${c.id}" placeholder="${c.appearingSeason}" name="appearingseason">
                                </div>
                                <div class="form-group">
                                    <div class="form-check">
                                        <input class="form-check-input" type="checkbox" id="gridCheck" name="protagonist">
                                        <label class="form-check-label" for="gridCheck">Protagonist</label>
                                    </div>
                                </div>
                            </form>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                                <button id="updatebtn-${c.id}" type="button" class="btn btn-primary" onclick = "fetch_update();">Save changes</button>
                            </div>
                            </div>
                        </div>
                        </div>
                    </p>
                    </div>
                  </div>`;
                    return elem;
                });
                document.getElementById('list-characters').innerHTML= lista.join(' ');
            }
            if(response.status === 401){
                alert("Session has expired, please log in again.")
                window.location.href = "login_registration.html";
            }
        }
        catch(error){
            console.error(error.message);
        }
    });
    showCharacters();
    
    var sort = function(criteria){
        debugger;
        showCharacters(criteria);
    }
    var sortByName = function(event){
        event.preventDefault();
        sort("name");};
    document.getElementById("dropdown-name").addEventListener('click',sortByName);
    var sortByAge = function(event){
        event.preventDefault();
        sort("age");};
    document.getElementById("dropdown-age").addEventListener('click',sortByAge);
    var sortByAppearingSeason = function(event){
        event.preventDefault();
        sort("appearingSeason");};
    document.getElementById("dropdown-appearingSeason").addEventListener('click',sortByAppearingSeason);
});

async function fetch_create(){
    const baseurl = "https://localhost:44319";
    const event = window.event;
    event.preventDefault();
    var frm = document.getElementById("frm-create");
    var data = {
        Name : frm.name.value.length != 0? frm.name.value: null,
        Age : parseInt(frm.age.value),
        Occupation : frm.occupation.value.length != 0? frm.occupation.value: null,
        isProta : frm.protagonist.checked,
        appearingSeason : parseInt(frm.appearingseason.value)
    };
    var params = {
        method : "POST",
        body : JSON.stringify(data),
        headers: { "Content-Type": "application/json; charset=utf-8", "Authorization":`Bearer ${jwt}`}
    };
    debugger;
    var response = await fetch(`${baseurl}/api/character`, params);
    try {
        if(response.status === 201){
            var json = await response.json();
            if(Boolean(json)){
                alert("The Character was creeated successfully.");
            }
            else{
                alert("Something happend, The changes could not be applied.");
            }
            window.location.href = "characters.html";
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

async function fetch_update(){
    debugger;
    const baseurl = "https://localhost:44319";
    const event = window.event;
    event.preventDefault();
    var id = event.target.id;
    id = parseInt(id.split('-')[1]);
    var frm = document.getElementById(`updatefrm-${id}`);
    var data = {
        Name : frm.name.value.length != 0? frm.name.value: null,
        Age : parseInt(frm.age.value),
        Occupation : frm.occupation.value.length != 0? frm.occupation.value: null,
        isProta : frm.protagonist.checked,
        appearingSeason : parseInt(frm.appearingseason.value)
    };
    var params = {
        method : "PUT",
        body : JSON.stringify(data),
        headers: { "Content-Type": "application/json; charset=utf-8", "Authorization":`Bearer ${jwt}`}
    };
    var response = await fetch(`${baseurl}/api/character/${id}`, params);
    try {
        if(response.status === 200){
            var json = await response.json();
            if(Boolean(json)){
                alert("The changes was applied.");
            }
            else{
                alert("Something happend, The changes could not be applied.");
            }
            window.location.href = "characters.html";
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

async function fetch_delete(){
    debugger;
    const baseurl = "https://localhost:44319";
    const event = window.event;
    event.preventDefault();
    var id = event.target.id;
    id = parseInt(id.split('-')[1]);

    var params = {
        method: "DELETE",
        headers: {"Authorization":`Bearer ${jwt}`}
    };
    var response = await fetch(`${baseurl}/api/character/${id}`,params);
    try {
        if(response.status == 200){
            var json = await response.json();
            if(Boolean(json)){
                alert('The Character was deleted.');
            }
            else{
                alert('Something happend, the team was not deleted');
            }
            window.location.href = 'characters.html';
        }
        else{
            throw new error(await response.text());
        }
    } catch (error) {
        console.error(error.message);
    }
};
