window.addEventListener('load',(event)=>{
    function showLogin(){
        document.getElementById("register").innerHTML="";
        document.getElementById("login").innerHTML=`
        <h4>Log In</h4>
        <form id="frm-login">
            <div class="form-group">
            <label for="exampleInputEmail1">Email address</label>
            <input type="email" class="form-control" id="exampleInputEmail1" aria-describedby="emailHelp" placeholder="Enter email">
            </div>
            <div class="form-group">
            <label for="exampleInputPassword1">Password</label>
            <input type="password" class="form-control" id="exampleInputPassword1" placeholder="Password">
            </div>
            <button type="submit" class="btn btn-primary">Submit</button>
            <button id ="tbn-register" class="btn btn-secondary" onclick = "showRegister();">Register</button>
        </form>
        `;

    };
    showLogin();

    function registerAccount(event){
        showLogin();
    }

    function showRegister(){
        document.getElementById("login").innerHTML="";
        document.getElementById("register").innerHTML=`
            <h4>Register</h4>
            <form id="frm-register">
                <div class="form-group">
                <label for="exampleInputEmail1">Email address</label>
                <input type="email" class="form-control" id="exampleInputEmail1" aria-describedby="emailHelp" placeholder="Enter email">
                </div>
                <div class="form-group">
                <label for="exampleInputPassword1">Password</label>
                <input type="password" class="form-control" id="exampleInputPassword1" placeholder="Password">
                </div>
                <button type="submit" class="btn btn-primary">Confirm</button>
            </form>
        `;
        document.getElementById("frm-register").addEventListener('submit',registerAccount);
    }
});