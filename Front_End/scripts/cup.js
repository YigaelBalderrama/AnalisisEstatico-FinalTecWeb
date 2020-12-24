const baseurl="https://localhost:44319";
if(!Boolean(sessionStorage.getItem("jwt"))){
    window.location.href = "login_registration.html";
}
const jwt = sessionStorage.getItem("jwt");

window.addEventListener("load",(event) => {
    event.preventDefault();
    var showtopPhrases = (async function (criteria = "none") {
        debugger;
        var params = {
            method : "GET",
            headers: {"Authorization":`Bearer ${jwt}`}
        };
        try {
            let botones=`<button onclick="topbySeason()" id="season-all" type="button" class="btn btn-info">All</button>`;
            for (let index = 1; index < 33; index++)
            {
                botones = botones + `<button onclick="topbySeason(${index})" id="season-${index}" type="button" class="btn btn-info">${index}</button>`;
            }
            document.getElementById('botonestemp').innerHTML= `<h5 style = "border-radius: 10px; background-color:white; padding:0 20px; font-family: 'Luckiest Guy', cursive; font-size: large;">Seasons</h5>${botones}`;
            topbySeason();
            if(response.status === 401){
                alert("Session has expired, please log in again.")
                window.location.href = "login_registration.html";
            }

        } catch (error) {
            console.error(error.message);
        }
    });
    showtopPhrases();
});
async function getImgRoute(idcharacter) {
    var params = {
        method : "GET",
        headers: {"Authorization":`Bearer ${jwt}`}
    };
    var response = await fetch(`${baseurl}/api/character/${idcharacter}`,params);
    try {
        debugger;
        if (response.status===200) {
            c = await response.json(); 
            return ` <img class="card-img-top logo" src="./imgs/logo-${c.name}.png" alt="Card image ${c.name}" style="width:70px; height:110px; margin: 0%; display:inline ">`;
        }
        if(response.status === 401){
            alert("Session has expired, please log in again.")
            window.location.href = "login_registration.html";
        }
        else{
            throw new error( await response.text())
        }
    } catch (error) {
        console.error(error.message);
    }
}
async function topbySeason(idseason=0)
{
    debugger;
    //event.preventDefault();
    var params = {
        method : "GET",
        headers: {"Authorization":`Bearer ${jwt}`}
    };
    var response = await fetch(`${baseurl}/api/character/phrases`,params);
    try {
        if (response.status ===200) {
            
            data =await response.json();
            if(idseason!==0)
            {
                data = data.filter(p=>p.season===idseason);
            }
            data = data.sort((o1,o2)=>(o1.likes>o2.likes)?-1:((o2.likes>o1.likes)?1:0));
            let podio="";let colors=['#EABE3F','#BEBEBE','#CD7F32'];let imagenchar="";

            for (let index = 0; index < Math.min(data.length,3); index++) {
                imagenchar = await getImgRoute(data[index].characterID);
                podio = podio +
                `
                <div style = " background-color:${colors[index]}; padding:10px; border-radius:10px; list-style-type: none;">
                    ${imagenchar}
                    ${data[index].content.toUpperCase()}
                    <div class="like" style="display:inline;"> 
                        <img name="none" src="../imgs/logo-like.png" style="width: 20px;"></img> 
                        <i>${data[index].likes} Likes</i>
                        <i>${data[index].season} Temporada</i>
                    </div>
                </div>  <br><br>`;
            }
            if (data.length===0) {
                podio=`
                <div class="bg-danger" style = "padding:10px; border-radius:10px; list-style-type: none;">
                <p>Theres no phrases registered yet</p>
                <div class="like" style="display:inline;"> 
                    <img name="none" src="../imgs/logo-like.png" style="width: 20px;"></img> 
                    <i> 0 Likes</i>
                    <i>${idseason}Temporada</i>
                </div>
            </div>  `;
            }
            document.getElementById('list-cup').innerHTML= podio;
        }
        if(response.status === 401){
            alert("Session has expired, please log in again.")
            window.location.href = "login_registration.html";
        }
        else{
            throw new error( await response.text());
        }
    } catch (error) {
        console.error(error.message);
    }

    
};