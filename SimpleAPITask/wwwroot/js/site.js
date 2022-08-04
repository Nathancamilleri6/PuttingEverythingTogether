let projects = [];

async function getProjectsForTable() {
    let email = getCookie("email");

    let user = await fetch(`api/users/${email}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
    }).then(response => response.json())

    fetch(`api/projects/${user.id}`)
        .then(response => response.json())
        .then(data => displayProjects(data))
        .catch(error => console.error('Unable to get projects.', error));
}

function getCookie(cname) {
    let name = cname + "=";
    let ca = document.cookie.split(';');
    for(let i = 0; i < ca.length; i++) {
      let c = ca[i];
      while (c.charAt(0) == ' ') {
        c = c.substring(1);
      }
      if (c.indexOf(name) == 0) {
        return c.substring(name.length, c.length);
      }
    }
    return "";
  }

function getUsersForTable() {
    fetch('api/users')
        .then(response => response.json())
        .then(data => displayUsers(data))
        .catch(error => console.error('Unable to get users.', error));
}

function getTagsForTable() {
    fetch('api/tags')
        .then(response => response.json())
        .then(data => displayTags(data))
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

function addTag() {
    const addTagTextbox = document.getElementById('add-tagName');

    const item = {
        name: addTagTextbox.value.trim(),
    };

    fetch('api/tags', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => {
            getTagsForTable();
            addTagTextbox.value = '';
        })
        .catch(error => console.error('Unable to add Tag.', error));
}

function addTagToProject() {
    const addTagIdTextbox = document.getElementById('tagToAdd');
    const addProjectIdTextbox = document.getElementById('tag-projectId');
    const projectId = parseInt(addProjectIdTextbox.value.trim());
    const tagId = parseInt(addTagIdTextbox.value.trim());

    const item = {
        projectId: projectId,
        tagId: tagId,
    };

    fetch(`api/tagProjects`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => {
            getProjectsForTable();
            populateTagList(projectId);
            getUsersForAssignDropdown(projectId);
            addTagIdTextbox.value = '';
            addProjectIdTextbox.value = '';
        })
        .catch(error => console.error('Unable to add tag to project.', error));

        closeAssignUserForm();
        return false;
}

function addAssignee() {
    const addUserIdTextbox = document.getElementById('userToAssign');
    const addProjectIdTextbox = document.getElementById('assign-projectId');
    const projectId = parseInt(addProjectIdTextbox.value.trim());
    const assigneeId = parseInt(addUserIdTextbox.value.trim())

    const item = {
        assigneeId: assigneeId,
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

        closeAssignUserForm();
        return false;
}

function populateAssigneeList(id){
    var assigneesList = document.getElementById("modalProjectAssignees");
    assigneesList.innerHTML = '';

    var assignees = getAllAssigneesByProjectId(id);
    assignees.then(function (assigneesResult) {
        document.getElementById("modalProjectAssigneesOuter").style.display = "block";
        assigneesResult.forEach((item) => {
            let li = document.createElement("li");
            li.innerText = item.userName;

            assigneesList.appendChild(li);
        });
    });
}

function populateTagList(id){
    var tagsList = document.getElementById("modalProjectTags");
    tagsList.innerHTML = '';

    var tags = getAllTagsByProjectId(id);
    tags.then(function (tagsResult) {
        document.getElementById("modalProjectTagsOuter").style.display = "block";
        tagsResult.forEach((item) => {
            let li = document.createElement("li");
            li.innerText = item.name;
            tagsList.appendChild(li);
        });
    });
}

function populateCommentsList(id){
    document.getElementById('comment-projectId').value = id;
    var commentsList = document.getElementById("modalProjectComments");
    commentsList.innerHTML = '';

    var comments = getAllCommentsByProjectId(id);
    comments.then(function (commentsResult) {
            document.getElementById("modalProjectCommentsOuter").style.display = "block";
            commentsResult.forEach((comment) => {
                if (comment !== null) {
                    let li = document.createElement("li");

                    var date = new Date(comment.dateTime);
                    var commentator = findUserById(comment.commentatorId);
                    commentator.then(function (commentatorResult) {
                        li.innerText = "Commentator: " + commentatorResult.userName +
                            "\nComment: " + comment.value +
                            "\nDate & Time: " + date + "\n";

                        let deleteButton = document.createElement('button');
                        deleteButton.setAttribute("class", "btn")
                        deleteButton.appendChild(document.createTextNode(" Delete "));
                        li.appendChild(deleteButton).addEventListener("click", function(){
                            deleteComment(comment);
                        });
                    })

                    commentsList.appendChild(li);
                }
            });
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

    closeAddCommentForm();

    return false;
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

    closeEditForm();

    return false;
}

function closeEditForm() {
    document.getElementById('editForm').style.display = 'none';
}

function closeAddCommentForm() {
    document.getElementById('addCommentForm').style.display = 'none';
}

function closeAssignUserForm() {
    document.getElementById('assignUserForm').style.display = 'none';
}

function closeAddTagForm() {
    document.getElementById('addTagForm').style.display = 'none';
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

async function getTags(projectId) {
    try {
        const response = await fetch(`api/tagProjects/${projectId}`);
        return await response.json();
    } catch (error) {
        return console.error('Unable to get Tags.', error);
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

async function getNonProjectTags(projectId) {
    try {
        const response = await fetch(`api/tagProjects/${projectId}/inverse`);
        return await response.json();
    } catch (error) {
        return console.error('Unable to get Non-Project Tags.', error);
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

const getAllTagsByProjectId = async (projectId) => {
    const tags = await getTags(projectId);
    if (tags) {
        return await tags;
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
    document.getElementById('assign-projectId').value = id;

    var project = projects.find(item => item.id === id);

    var users = await getNonAssignees(id);
    users = users.filter(user => user.id !== project.creatorId)
    users.forEach(user => {
                userSel.options[userSel.options.length] = new Option(user.userName, user.id);  
    })

    if(users.length === 0){
        document.getElementById("modalProjectAssigneesOuter").style.display = "none";
    }
    else {
        document.getElementById("modalProjectAssigneesOuter").style.display = "block";
    }
}

async function getTagsForTagDropdown(id) {
    var tagSel = document.getElementById("tagToAdd");
    emptyDropdownList(tagSel)
    document.getElementById('tag-projectId').value = id;

    var tags = await getNonProjectTags(id);
    tags.forEach(tags => {
        tagSel.options[tagSel.options.length] = new Option(tags.name, tags.id);  
    })

    if(tags.length === 0){
        document.getElementById("modalProjectTagsOuter").style.display = "none";
    }
    else {
        document.getElementById("modalProjectTagsOuter").style.display = "block";
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
        .then(() => { 
            getProjectsForTable();
            populateCommentsList(comment.projectId); 
        })
        .catch(error => console.error('Unable to delete comment.', error));
}

function displayEditForm(id) {
    var item = projects.find(item => item.id === id);

    document.getElementById('edit-projectId').value = item.id;
    document.getElementById('edit-projectName').value = item.name;
    document.getElementById('editForm').style.display = 'block';
}

function displayAddCommentForm(id) {
    var item = projects.find(item => item.id === id);

    document.getElementById('comment-projectId').value = item.id;
    document.getElementById('addCommentForm').style.display = 'block';
}

function displayAssignUserForm(id) {
    var item = projects.find(item => item.id === id);

    document.getElementById('assign-projectId').value = item.id;
    document.getElementById('assignUserForm').style.display = 'block';
}

function displayAddTagForm(id) {
    var item = projects.find(item => item.id === id);

    document.getElementById('tag-projectId').value = item.id;
    document.getElementById('addTagForm').style.display = 'block';
}

function displayProjectDetails(id) {
    document.getElementById('addCommentForm').style.display = 'none';
    document.getElementById('assignUserForm').style.display = 'none';
    document.getElementById('addTagForm').style.display = 'none';
    // Get the modal
    var modal = document.getElementById("projectModal");
    modal.style.display = "block";

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
    getTagsForTagDropdown(id);

    var creator = findUserById(item.creatorId);
    creator.then(function (creatorResult) {
        document.getElementById("modalProjectCreator").innerHTML = creatorResult.userName;
    })

    populateAssigneeList(item.id);
    var users = getNonAssignees(id);
    users.then(function (usersResult){
        usersResult = usersResult.filter(user => user.id !== item.creatorId);
        if(usersResult.length > 0){
            var modalProjectAssigneesOuter = document.getElementById("modalProjectAssigneesOuter");
            let assignUserButton = document.createElement('button');
            modalProjectAssigneesOuter.appendChild(assignUserButton);
            assignUserButton.setAttribute("id", "assignUserButton")
            assignUserButton.innerText = 'Assign User';
            assignUserButton.setAttribute('onclick', `displayAssignUserForm(${item.id})`);
        }
    })

    populateCommentsList(item.id);
    var modalProjectCommentsOuter = document.getElementById("modalProjectCommentsOuter");
    let addCommentButton = document.createElement('button');
    modalProjectCommentsOuter.appendChild(addCommentButton);
    addCommentButton.setAttribute("id", "addCommentButton")
    addCommentButton.innerText = 'Add Comment';
    addCommentButton.setAttribute('onclick', `displayAddCommentForm(${item.id})`);

    populateTagList(item.id);
    var tags = getNonProjectTags(id);
    tags.then(function (tagsResult){
        // tagsResult = tagsResult.filter(tag => tag.projectId !== item.id);
        if(tagsResult.length > 0){
            var modalProjectTagsOuter = document.getElementById("modalProjectTagsOuter");
            let addTagButton = document.createElement('button');
            modalProjectTagsOuter.appendChild(addTagButton);
            addTagButton.setAttribute("id", "addTagButton")
            addTagButton.innerText = 'Add tag';
            addTagButton.setAttribute('onclick', `displayAddTagForm(${item.id})`);
        }
    })

    // Get the <span> element that closes the modal
    var span = document.getElementsByClassName("close")[0];
    span.onclick = function () {
        closeModal()
    }

    // When the user clicks anywhere outside of the modal, close it
    window.onclick = function (event) {
        if (event.target == modal) {
            closeModal()
        }
    }
}

function closeModal(){
    var modal = document.getElementById("projectModal");
    modal.style.display = "none";
    closeEditForm();
    closeAddCommentForm();
    closeAssignUserForm();
    closeAddTagForm();
    document.getElementById('editButton').remove();
    document.getElementById('addCommentButton').remove();
    if(document.getElementById('assignUserButton')){
        document.getElementById('assignUserButton').remove();
    }
    document.getElementById('addTagButton').remove();
    document.getElementById("modalProjectAssigneesTitle").style.display = "block";
    document.getElementById("modalProjectCommentsOuter").style.display = "block";
    emptyDropdownList(document.getElementById("userToAssign"))
    emptyDropdownList(document.getElementById("tagToAdd"))
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

        // let deleteButton = button.cloneNode(false);
        // deleteButton.innerText = 'Tag';
        // deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

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
        var userCreatedDate = new Date(item.createdDate)
        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(document.createTextNode(item.userName));

        let td2 = tr.insertCell(1);
        td2.appendChild(document.createTextNode(item.email));

        let td3 = tr.insertCell(2);
        td3.appendChild(document.createTextNode(userCreatedDate));
    });

    projects = data;
}

function displayTags(data) {
    const tBody = document.getElementById('tags');
    tBody.innerHTML = '';

    data.forEach(item => {
        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(document.createTextNode(item.name));
    });

    projects = data;
}