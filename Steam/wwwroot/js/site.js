async function showGameDetails(id = 0) {
    await fetch('/Game/GetById?id=' + id, { method: "GET" })
    .then(data => {
        window.location.href = '/Game/GetById?id=' + id;
    })
}

async function DeleteNotification(id) {
    await fetch('/Notification/Delete?id=' + id, { method: "DELETE" })
        .then(data => {
            window.location.href = '/Notification/GetAllNotification';
        })
}

async function GoToWithId(url,id) {
    await fetch(url + '?id=' + id, { method: "GET" })
        .then(data => {
            window.location.href = url + '?id=' + id;
        })
}
async function Ban(id = '') {
    await fetch('/Admin/Ban?id=' + id, { method: "DELETE" })
        .then(data => {
            window.location.href = '/Admin/AllUser';
        })
}

async function Buy(id) {
    await fetch('/Game/Buy?id=' + id, { method: "POST" })
        .then(data => {
            window.location.href = '/User/Profile';
        })
}

async function UpdateGroupView(id) {
    await fetch('/Group/Update?id=' + id, { method: "GET" })
        .then(data => {
            window.location.href = '/Group/Update?id=' + id;
        })
}

async function updateUserData(id) {
    await fetch('/User/Update', { method: "GET" })
        .then(data => {
            window.location.href = '/User/Update';
        })
}

async function updateUser()
{
    event.preventDefault();
    var data = $("#updateUser").serializeArray().reduce(function (obj, item) {
        obj[item.name] = item.value;
        return obj;
    }, {});
    var jsonBody = JSON.stringify(data);
    console.log(jsonBody);
    await fetch('/User/Update', {
        method: "PUT",
        body: jsonBody,
        headers: {
            "Content-Type": "application/json"
        }      
    }).then(data => {
        window.location.href = '/User/Profile';
    })
}

async function UpdateGroup(id) {
    event.preventDefault();
    var data = $("#updateGroupContainer").serializeArray().reduce(function (obj, item) {
        obj[item.name] = item.value;
        return obj;
    }, {});
    var jsonBody = JSON.stringify(data);

    await fetch('/Group/Update?id=' + id, {
        method: "PUT",
        body: jsonBody,
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then(data => {
            window.location.href = '/Group/GetAll';
        });
}

async function DeleteGroup(id) {
    await fetch('/Group/DeleteGroup?id=' + id, { method: "DELETE" })
        .then(data => {
            window.location.href = '/Group/GetAll';
        })
}
async function Leave(id) {
    await fetch('/Group/Leave?id=' + id, { method: "DELETE" })
        .then(data => {
            window.location.href = '/Group/ShowJoinedGroup';
        })
}
async function JoinIn(id) {
    await fetch('/Group/JoinIn?id=' + id, { method: "POST" })
        .then(data => {
            window.location.href = '/Group/GetAll';
        })
}
async function AddGroup() {
    await fetch('/Group/Add', { method: "GET" })
        .then(data => {
            window.location.href = '/Group/Add';
        })
}

async function ShowGroup() {
    await fetch('/Group/GetUserGroup', { method: "GET" })
        .then(data => {
            window.location.href = '/Group/GetUserGroup';
        })
}

async function showGroupDetail(id) {
    await fetch('/Group/GetById?id=' + id, { method: "GET" })
        .then(data => {
            window.location.href = '/Group/GetById?id=' + id;
        })
}

async function Back(url = '/') {
    await fetch(url, { method: "GET" })
        .then(data => {
           window.location.href = url;
        })
}

async function showWorkShopDetails(id = 0) {
    await fetch('/WorkShop/GetById?id=' + id, { method: "GET" })
        .then(data => {
            window.location.href = '/WorkShop/GetById?id=' + id;
        })
}


async function UpdateGame(id) {   
    console.log(1);
    event.preventDefault();
    var data = $("#gameFormUpdate").serializeArray().reduce(function (obj, item) {
        obj[item.name] = item.value;
        return obj;
    }, {});
    var jsonBody = JSON.stringify(data);


    await fetch('/Game/Update?id=' + id, {
        method: "PUT",
        body: jsonBody,
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then(data => {
            window.location.href = '/Game/GetAll';
        });
}

async function UpdateWorkShop(id) {
    event.preventDefault();
    var data = $("#WorkShopContainer").serializeArray().reduce(function (obj, item) {
        obj[item.name] = item.value;
        return obj;
    }, {});
    var jsonBody = JSON.stringify(data);

    await fetch('/WorkShop/Update?id=' + id, {
        method: "PUT",
        body: jsonBody,
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then(data => {
            window.location.href = '/WorkShop/GetAll';
        });
}

async function UpdateWorkShopView(id) {
    await fetch('/WorkShop/Update?id=' + id, { method: "GET" })
        .then(data => {
            window.location.href = '/WorkShop/Update?id=' + id;
        })
}
async function UpdateGameView(id){
    await fetch('/Game/Update?id=' + id, { method: "GET" })
        .then(data => {
            window.location.href = '/Game/Update?id=' + id;
        })
}
async function DeleteGame(id) {
    await fetch('/Game/Delete?id=' + id, {
        method: "DELETE"
    })
        .then(data => {
            window.location.href = '/Game/GetAll';
        })
}

async function DeleteWorkShop(id) {
    await fetch('/WorkShop/Delete?id=' + id, {
        method: "DELETE"
    }).then(data => {
        window.location.href = '/WorkShop/GetAll';
    }) 
}

async function AddWorkShop() {
    await fetch('/WorkShop/Add', { method: "GET" })
        .then(data => {
            window.location.href = '/WorkShop/Add';
        })
}

async function ShowWorkShop() {
    await fetch('/WorkShop/GetUserWorkShop', { method: "GET" })
        .then(data => {
            window.location.href = '/WorkShop/GetUserWorkShop';
        })
}

async function AddSub(id) {
    await fetch('/WorkShop/AddToSub?id=' + id, { method: "POST" })
        .then(data => {
            window.location.href = '/WorkShop/ShowSub';
        })
        
}

async function UnFollow(id) {
    await fetch('/WorkShop/UnFollow?id=' + id, { method: "DELETE" })
        .then(data => {
            window.location.href = '/WorkShop/ShowSub';
        })
}