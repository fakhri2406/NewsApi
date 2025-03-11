const baseUrl = window.location.origin + '/api';

let jwtToken = '';
let isEditing = false;

async function fetchNews() {
    try {
        const response = await fetch(`${baseUrl}/News`, {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer ' + jwtToken
            }
        });
        if (response.ok) {
            const newsItems = await response.json();
            renderNewsList(newsItems);
        } else {
            const errorMsg = await response.text();
            document.getElementById('newsList').innerText = 'Error: ' + errorMsg;
        }
    } catch (error) {
        document.getElementById('newsList').innerText = 'Error: ' + error;
    }
}

function renderNewsList(newsItems) {
    const newsList = document.getElementById('newsList');
    newsList.innerHTML = '';
    if (newsItems.length === 0) {
        newsList.innerHTML = '<p>No news available.</p>';
    } else {
        newsItems.forEach(news => {
            const item = document.createElement('div');
            item.className = 'list-group-item';
            item.innerHTML = `
        <div class="d-flex w-100 justify-content-between align-items-center">
          <div>
            <h5 class="mb-1">${news.title}</h5>
            <p class="mb-1">${news.content}</p>
            <small>Published: ${new Date(news.publishedDate).toLocaleString()}</small>
          </div>
          <div>
            <button class="btn btn-sm btn-secondary editNewsBtn" data-id="${news.id}" data-title="${news.title}" data-content="${news.content}">Edit</button>
            <button class="btn btn-sm btn-danger deleteNewsBtn" data-id="${news.id}">Delete</button>
          </div>
        </div>
      `;
            newsList.appendChild(item);
        });
    }
}

document.getElementById('registerForm').addEventListener('submit', async (e) => {
    e.preventDefault();
    const username = document.getElementById('registerUsername').value;
    const password = document.getElementById('registerPassword').value;
    const roleId = document.getElementById('registerRoleId').value;

    const formData = new FormData();
    formData.append('Username', username);
    formData.append('Password', password);
    formData.append('RoleId', roleId);

    try {
        const response = await fetch(`${baseUrl}/Auth/register`, {
            method: 'POST',
            body: formData
        });
        const result = await response.text();
        document.getElementById('registerMessage').innerText = result;
    } catch (error) {
        document.getElementById('registerMessage').innerText = 'Error: ' + error;
    }
});

document.getElementById('loginForm').addEventListener('submit', async (e) => {
    e.preventDefault();
    const username = document.getElementById('loginUsername').value;
    const password = document.getElementById('loginPassword').value;

    const formData = new FormData();
    formData.append('Username', username);
    formData.append('Password', password);

    try {
        const response = await fetch(`${baseUrl}/Auth/login`, {
            method: 'POST',
            body: formData
        });
        if (response.ok) {
            jwtToken = await response.text();
            document.getElementById('loginMessage').innerText = 'Login successful! Token saved.';
        } else {
            const errorMsg = await response.text();
            document.getElementById('loginMessage').innerText = 'Login failed: ' + errorMsg;
        }
    } catch (error) {
        document.getElementById('loginMessage').innerText = 'Error: ' + error;
    }
});

document.getElementById('fetchNewsBtn').addEventListener('click', fetchNews);

document.getElementById('addNewsBtn').addEventListener('click', () => {
    isEditing = false;
    document.getElementById('newsModalLabel').innerText = 'Add News';
    document.getElementById('newsId').value = '';
    document.getElementById('newsTitle').value = '';
    document.getElementById('newsContent').value = '';
    document.getElementById('newsModalMessage').innerText = '';
    $('#newsModal').modal('show');
});

document.getElementById('newsForm').addEventListener('submit', async (e) => {
    e.preventDefault();
    const newsId = document.getElementById('newsId').value;
    const title = document.getElementById('newsTitle').value;
    const content = document.getElementById('newsContent').value;

    const newsData = { title, content };

    if (!isEditing) {
        try {
            const response = await fetch(`${baseUrl}/News`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + jwtToken
                },
                body: JSON.stringify(newsData)
            });
            if (response.ok) {
                $('#newsModal').modal('hide');
                fetchNews();
            } else {
                const errorMsg = await response.text();
                document.getElementById('newsModalMessage').innerText = 'Error: ' + errorMsg;
            }
        } catch (error) {
            document.getElementById('newsModalMessage').innerText = 'Error: ' + error;
        }
    } else {
        try {
            const response = await fetch(`${baseUrl}/News/${newsId}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + jwtToken
                },
                body: JSON.stringify({ id: parseInt(newsId), title, content })
            });
            if (response.ok) {
                $('#newsModal').modal('hide');
                fetchNews();
            } else {
                const errorMsg = await response.text();
                document.getElementById('newsModalMessage').innerText = 'Error: ' + errorMsg;
            }
        } catch (error) {
            document.getElementById('newsModalMessage').innerText = 'Error: ' + error;
        }
    }
});

document.getElementById('newsList').addEventListener('click', (e) => {
    if (e.target.classList.contains('editNewsBtn')) {
        isEditing = true;
        const newsId = e.target.getAttribute('data-id');
        const title = e.target.getAttribute('data-title');
        const content = e.target.getAttribute('data-content');
        document.getElementById('newsModalLabel').innerText = 'Edit News';
        document.getElementById('newsId').value = newsId;
        document.getElementById('newsTitle').value = title;
        document.getElementById('newsContent').value = content;
        document.getElementById('newsModalMessage').innerText = '';
        $('#newsModal').modal('show');
    }
});

document.getElementById('newsList').addEventListener('click', async (e) => {
    if (e.target.classList.contains('deleteNewsBtn')) {
        const newsId = e.target.getAttribute('data-id');
        if (confirm('Are you sure you want to delete this news item?')) {
            try {
                const response = await fetch(`${baseUrl}/News/${newsId}`, {
                    method: 'DELETE',
                    headers: {
                        'Authorization': 'Bearer ' + jwtToken
                    }
                });
                if (response.ok) {
                    fetchNews();
                } else {
                    const errorMsg = await response.text();
                    alert('Delete failed: ' + errorMsg);
                }
            } catch (error) {
                alert('Error: ' + error);
            }
        }
    }
});