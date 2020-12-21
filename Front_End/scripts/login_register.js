// window.addEventListener("load",(event)=>{

var login=async function(event){
    
    event.preventDefault();
    var form = event.currentTarget;
    if(!(form.email.value && form.password.value)){
        alert("Please fill the values.");
        return;
    }
    var data = {
        Email: form.email.value,
        Password: form.password.value
    };
    const baseUrl = "https://localhost:44319";
    const url = `${baseUrl}/api/auth/login`;
    var response = await fetch(url, {
            headers: { "Content-Type": "application/json; charset=utf-8" },
            method: 'POST',
            body: JSON.stringify(data)
        });
    debugger;
    try {
        if (response.status === 200) {
            var data = await response.json();
            debugger;
            sessionStorage.setItem("jwt", data.message);
            window.history.back();
        } else {
            if(response.status === 400){
                var data = await response.json();
                alert(data.message);
                window.location.href = "login_registration.html"
            }
            response.text().then((data) => {
                debugger;
                console.log(data);
            });
        }
    } catch (error) {
       debugger;
       console.log(error);
    }
};

var showRegister = function(){
    debugger;
    document.getElementById("login").style.padding ="0";
    document.getElementById("login").innerHTML="";
    document.getElementById("register").innerHTML=`
        <h4>Register</h4>
        <form id="frm-register">
            <div class="form-group">
            <label for="exampleInputEmail1">Email address</label>
            <input type="email" class="form-control"  aria-describedby="emailHelp" placeholder="Enter email" name = "email">
            </div>
            <div class="form-group">
            <label for="exampleInputPassword1">Password</label>
            <input type="password" class="form-control" placeholder="Password" name="password">
            </div>
            <div class="form-group">
            <label for="exampleInputPassword2">Confirm Password</label>
            <input type="password" class="form-control" placeholder="confirm password name="c_password">
            </div>
            <button type="submit" class="btn btn-primary">Confirm</button>
            <button id ="tbn-cancel" class="btn btn-secondary" onclick = "window.location.href = login_registration.html;">Cancel</button>
        </form>
    `;
    document.getElementById("register").style.padding ="5%";
    document.getElementById("frm-register").addEventListener('submit',registerAccount);
};
function showLogin(){
    document.getElementById("register").style.padding ="0";
    document.getElementById("register").innerHTML="";
    document.getElementById("login").innerHTML=`
    <h4>Log In</h4>
    <form id="frm-login">
        <div class="form-group">
        <label for="exampleInputEmail1">Email address</label>
        <input type="email" class="form-control" id="exampleInputEmail1" aria-describedby="emailHelp" placeholder="Enter email" name="email">
        </div>
        <div class="form-group">
        <label for="exampleInputPassword1">Password</label>
        <input type="password" class="form-control" id="exampleInputPassword1" placeholder="Password" name="password">
        </div>
        <button type="submit" class="btn btn-primary">Submit</button>
        <button id ="tbn-register" class="btn btn-secondary" onclick = "showRegister();">Register</button>
    </form>
    `;
    document.getElementById("login").style.padding ="5%";
    document.getElementById("frm-login").addEventListener('submit',login);
};

function registerAccount(event){
    showLogin();
}
    
// });
showRegister();
showLogin();