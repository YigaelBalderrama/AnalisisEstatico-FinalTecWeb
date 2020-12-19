window.addEventListener("load",(event) =>{
    
    const baseurl = "https://localhost:44319";
    (async function () {
        var response = await fetch(`${baseurl}/api/character`);
        try {
            if (response.status === 200){
                message = await response.json();
                lista = message.map((c) => {
                    let elem =   `<div class="card " style="width: 300px; margin-bottom:30px; border-radius:15px;">
                    <img class="card-img-top logo" src="./imgs/logo-${c.name}.png" alt="Card image ${c.name}" style="width:70%; height:200px; margin: 5% 15%">
                    <div class="card-body">
                    <h5>${c.name}</h5>  
                    <p class="card-text">
                      <b>Name</b>: ${c.name}<br>
                      <b>Age</b>: ${c.age}<br>
                      <b>Protagonist</b>: ${(c.protagonist)?"Protagonista":"No Protagonista"}<br>
                      <b>Appearing Season</b>: ${c.appearingSeason}<br><br>
                      <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#Modal${c.id}">
                        Update
                        </button>
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
                            <form id="frm-${c.id}">
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
                                    <input type="number" class="form-control" id="appearing${c.id}" placeholder="${c.appearingSeason}" name="appearingSeason">
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
                                <button id="button-${c.id}" type="button" class="btn btn-primary">Save changes</button>
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
        }
        catch(error){

        }
    })();
});

// var lista = [
//     {
//         id: 1,
//         name:"homero",
//         age: 39,
//         protagonist: true,
//         appearingSeason: 1,
//         occupation:"father"
//     },
//     {
//         id: 2,
//         name:"marge",
//         age: 36,
//         protagonist: true,
//         appearingSeason: 1,
//         occupation:"mother"
//     },
//     {
//         id: 3,
//         name:"bart",
//         age: 10,
//         protagonist: true,
//         appearingSeason: 1,
//         occupation:"son"
//     },
//     {
//         id: 4,
//         name:"lisa",
//         age: 8,
//         protagonist: true,
//         appearingSeason: 1,
//         occupation:"daughter"
//     }
// ];