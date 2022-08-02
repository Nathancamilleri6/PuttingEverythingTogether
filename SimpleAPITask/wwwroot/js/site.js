let projects = [];

function getProjectsForTable() {
    fetch('api/projects')
        .then(response => response.json())
        .then(data => displayProjects(data))
        .catch(error => console.error('Unable to get projects.', error));
}

function getUsersForTable() {
    fetch('api/users')
        .then(response => response.json())
        .then(data => displayUsers(data))
        .catch(error => console.error('Unable to get users.', error));
}

function addProject() {
    const addNameTextbox = document.getElementById('add-projectName');

    const item = {
        name: addNameTextbox.value.trim(),
        creatorId: 0
    };

    fetch('api/projects', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => {
            getProjectsForTable();
            addNameTextbox.value = '';
        })
        .catch(error => console.error('Unable to add project.', error));
}

function addAssignee() {
    const addUserIdTextbox = document.getElementById('userToAssign');
    const addProjectIdTextbox = document.getElementById('assign-projectId');
    const projectId = addProjectIdTextbox.value.trim()

    const item = {
        assigneeId: addUserIdTextbox.value.trim(),
        projectId: projectId,
    };

    fetch('api/assignees', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => {
            getProjectsForTable();
            populateAssigneeList(projectId);
            getUsersForAssignDropdown(projectId);
            addUserIdTextbox.value = '';
            addProjectIdTextbox.value = '';
        })
        .catch(error => console.error('Unable to add assignee.', error));
}

function populateAssigneeList(id){
    var assigneesList = document.getElementById("modalProjectAssignees");
    assigneesList.innerHTML = '';

    var assignees = getAllAssigneesByProjectId(id);
    assignees.then(function (assigneesResult) {
        if (assigneesResult.length === 0) {
            document.getElementById("modalProjectAssigneesTitle").style.display = "none";
        }
        assigneesResult.forEach((item) => {
            let li = document.createElement("li");
            li.innerText = item.userName;

            assigneesList.appendChild(li);
        });
    });
}

function populateCommentsList(id){
    var commentsList = document.getElementById("modalProjectComments");
    commentsList.innerHTML = '';

    var comments = getAllCommentsByProjectId(id);
    comments.then(function (commentsResult) {
        if (commentsResult.length < 1) {
            document.getElementById("modalProjectCommentsOuter").style.display = "none";
        }
        else {
            document.getElementById("modalProjectCommentsOuter").style.display = "block";
            commentsResult.forEach((comment) => {
                if (comment !== null) {
                    let li = document.createElement("li");
                    var commentator = findUserById(comment.commentatorId);
                    commentator.then(function (commentatorResult) {
                        li.innerText = "Commentator: " + commentatorResult.userName +
                            "\nComment: " + comment.value +
                            "\nDate & Time: " + comment.dateTime;

                        let deleteButton = document.createElement('button');
                        deleteButton.setAttribute("id", "deleteCommentBtn")
                        deleteButton.innerText = 'Delete';
                        li.appendChild(deleteButton);
                        deleteButton.setAttribute('onclick', `deleteComment(${comment})`);
                    })

                    commentsList.appendChild(li);
                }
            });
        }
    })
}

function addComment() {
    const addCommentTextbox = document.getElementById('comment-comment');
    const addProjectIdTextbox = document.getElementById('comment-projectId');
    const projectId = parseInt(addProjectIdTextbox.value);
    const date = new Date().toUTCString()

    const item = {
        value: addCommentTextbox.value.trim(),
        commentatorId: 0,
        projectId: projectId,
        date: date
    };

    fetch('api/comments', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => {
            getProjectsForTable();
            populateCommentsList(projectId);
            addCommentTextbox.value = '';
            addProjectIdTextbox.value = '';
        })
        .catch(error => console.error('Unable to add comment.', error));
}

function editProject() {
    const projectId = parseInt(document.getElementById('edit-projectId').value);
    const projectName = document.getElementById('edit-projectName').value;
    var projectToEdit = projects.find(item => item.id === projectId);

    const item = {
        id: projectId,
        name: projectName.trim(),
        creatorId: projectToEdit.creatorId
    }

    console.log(item);

    fetch(`api/projects/${projectId}`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => {
            getProjectsForTable();
            document.getElementById('modalProjectName').innerHTML = projectName
            populateAssigneeList(projectId);
            getUsersForAssignDropdown(projectId);
    })
        .catch(error => console.error('Unable to edit project.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

async function getUsers() {
    try {
        const response = await fetch('api/users');
        return await response.json();
    } catch (error) {
        return console.error('Unable to get Users.', error);
    }
}

const findUserById = async (id) => {
    const users = await getUsers();
    var userFound = users.find((user) => user.id === id);

    if (userFound) {
        return await userFound;
    } else {
        return null;
    }
}

async function getAssignees(projectId) {
    try {
        const response = await fetch(`api/assignees/${projectId}`);
        return await response.json();
    } catch (error) {
        return console.error('Unable to get Assignees.', error);
    }
}

async function getNonAssignees(projectId) {
    try {
        const response = await fetch(`api/assignees/${projectId}/inverse`);
        return await response.json();
    } catch (error) {
        return console.error('Unable to get Non-Assignees.', error);
    }
}

const getAllAssigneesByProjectId = async (projectId) => {
    const assignees = await getAssignees(projectId);
    if (assignees) {
        return await assignees;
    } else {
        return null;
    }
}

const getAllNonAssigneesByProjectId = async (projectId) => {
    const nonAssignees = await getNonAssignees(projectId);
    if (nonAssignees) {
        return await nonAssignees;
    } else {
        return null;
    }
}

async function getUsersForAssignDropdown(id) {
    var userSel = document.getElementById("userToAssign");
    emptyDropdownList(userSel)

    var project = projects.find(item => item.id === id);
    document.getElementById('assign-projectId').value = project.id;

    var users = await getNonAssignees(id);
    users = users.filter(user => user.id !== project.creatorId)
    users.forEach(user => {
                userSel.options[userSel.options.length] = new Option(user.userName, user.id);  
    })

    if(users.length === 0){
        document.getElementById("modalAssignUserOuter").style.display = "none";
    }
    else {
        document.getElementById("modalAssignUserOuter").style.display = "block";
    }
}

async function getComments(projectId) {
    try {
        const response = await fetch(`api/comments/${projectId}`);
        return await response.json();
    } catch (error) {
        return console.error('Unable to get comments.', error);
    }
}

const getAllCommentsByProjectId = async (projectId) => {
    const comments = await getComments(projectId);

    if (comments) {
        return await comments;
    } else {
        return null;
    }
}

function deleteComment(comment) {
    fetch(`api/comments/${comment.id}`, {
        method: 'DELETE'
    })
        .then(() => displayProjectDetails(comment.projectId))
        .catch(error => console.error('Unable to delete comment.', error));
}

function displayEditForm(id) {
    var item = projects.find(item => item.id === id);

    document.getElementById('edit-projectId').value = item.id;
    document.getElementById('edit-projectName').value = item.name;
    document.getElementById('editForm').style.display = 'block';
}

function displayProjectDetails(id) {
    console.log("Project Id: ", id)
    // Get the modal
    var modal = document.getElementById("projectModal");
    var modalContent = document.getElementById("modal-content");
    modal.style.display = "block";
    document.getElementById('comment-projectId').value = id;

    var item = projects.find(item => item.id === id);
    var projectNameOuter = document.getElementById("modalProjectNameOuter");
    var projectName = document.getElementById("modalProjectName");
    projectName.innerHTML = item.name;

    let editButton = document.createElement('button');

    projectNameOuter.appendChild(editButton);

    editButton.setAttribute("id", "editButton")
    editButton.innerText = 'Edit';
    editButton.setAttribute('onclick', `displayEditForm(${item.id})`);
    getUsersForAssignDropdown(id);

    var creator = findUserById(item.creatorId);
    creator.then(function (creatorResult) {
        document.getElementById("modalProjectCreator").innerHTML = creatorResult.userName;
    })

    populateAssigneeList(item.id);
    populateCommentsList(item.id);

    // Get the <span> element that closes the modal
    var span = document.getElementsByClassName("close")[0];
    span.onclick = function () {
        modal.style.display = "none";
        closeInput();
        document.getElementById('editButton').remove();
        document.getElementById("modalProjectAssigneesTitle").style.display = "block";
        document.getElementById("modalProjectCommentsOuter").style.display = "block";
        emptyDropdownList(document.getElementById("userToAssign"))
    }

    // When the user clicks anywhere outside of the modal, close it
    window.onclick = function (event) {
        if (event.target == modal) {
            modal.style.display = "none";
            closeInput();
            document.getElementById('editButton').remove();
            document.getElementById("modalProjectAssigneesTitle").style.display = "block";
            document.getElementById("modalProjectCommentsOuter").style.display = "none";
            emptyDropdownList(document.getElementById("userToAssign"))
        }
    }
}

function emptyDropdownList(dropdown){
    while(dropdown.options.length > 1){
        dropdown.remove(dropdown.options.length - 1)
    }
}

function displayProjects(data) {
    const tBody = document.getElementById('projects');
    const button = document.createElement('button');
    tBody.innerHTML = '';
    //_displayCount(data.length);

    data.forEach(item => {
        let viewButton = button.cloneNode(false);
        viewButton.innerText = 'View';
        viewButton.setAttribute('onclick', `displayProjectDetails(${item.id})`)

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Tag';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let tr = tBody.insertRow();

        let td2 = tr.insertCell(0);
        td2.appendChild(document.createTextNode(item.name));

        let td3 = tr.insertCell(1)
        creator = findUserById(item.creatorId);
        creator.then(function (result) {
            td3.appendChild(document.createTextNode(result.userName))
        })

        let td4 = tr.insertCell(2);
        td4.appendChild(viewButton);
    });

    projects = data;
}

function displayUsers(data) {
    const tBody = document.getElementById('users');
    tBody.innerHTML = '';

    data.forEach(item => {
        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(document.createTextNode(item.userName));

        let td2 = tr.insertCell(1);
        td2.appendChild(document.createTextNode(item.email));

        let td3 = tr.insertCell(2);
        td3.appendChild(document.createTextNode(item.createdDate));
    });

    projects = data;
}