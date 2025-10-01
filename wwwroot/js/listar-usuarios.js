let allUsers = [];
let sortState = {
    column: null,
    direction: 'asc'
};

const sortableColumns = [
    { headerText: 'ID', property: 'id', type: 'number' },
    { headerText: 'Nome', property: 'name', type: 'string' }
];


/**
 * Exibe os usuários na tabela.
 * @param {Array} users 
 */
function renderUsers(users) {
    const tbody = document.getElementById("users-table-body");
    tbody.innerHTML = "";

    users.forEach(user => {
        const row = document.createElement("tr");
        row.innerHTML = `
            <td>${user.id}</td>
            <td>${user.name}</td>
            <td>${user.email}</td>
            <td>********</td>
            <td>${user.phoneNumber || ""}</td>
            <td>${user.dateOfBirth || ""}</td>
            <td>${user.streetName || ""}</td>
            <td>${user.streetNumber || ""}</td>
            <td>${user.city || ""}</td>
            <td>${user.state || ""}</td>
            <td>${user.country || ""}</td>
        `;
        tbody.appendChild(row);
    });

    updateSortIcons();
}

async function loadUsers() {
    try {
        const response = await fetch("http://localhost:5229/api/users");
        if (!response.ok) throw new Error("Erro ao buscar usuários.");

        const users = await response.json();
        allUsers = users;
        renderUsers(allUsers);
        addSortListeners();
    } catch (err) {
        alert(err.message);
    }
}

/**
 * lógica de ordenação da tabela.
 * @param {string} column 
 * @param {string} type
 */
function sortTable(column, type) {
    //Se for a mesma coluna, inverte a direção
    if (sortState.column === column) {
        sortState.direction = sortState.direction === 'asc' ? 'desc' : 'asc';
    } else {
        //Se for uma nova coluna, define a coluna e a direção inicial como crescente
        sortState.column = column;
        sortState.direction = 'asc';
    }

    const directionMultiplier = sortState.direction === 'asc' ? 1 : -1;

    allUsers.sort((a, b) => {
        const aValue = a[column];
        const bValue = b[column];

        if (type === 'number') {
            return (aValue - bValue) * directionMultiplier;
        } else if (type === 'string') {
            const aStr = String(aValue || "").toLowerCase();
            const bStr = String(bValue || "").toLowerCase();

            if (aStr < bStr) return -1 * directionMultiplier;
            if (aStr > bStr) return 1 * directionMultiplier;
            return 0;
        }
        return 0;
    });

    renderUsers(allUsers);
}

function updateSortIcons() {
    const headerCells = document.querySelectorAll('.data-table thead th');

    headerCells.forEach(th => {
        th.classList.remove('sorted-asc', 'sorted-desc');
        th.querySelector('.sort-icon')?.remove();

        const columnInfo = sortableColumns.find(col => col.headerText === th.textContent);

        if (columnInfo && sortState.column === columnInfo.property) {
            const directionClass = `sorted-${sortState.direction}`;
            th.classList.add(directionClass);

            const icon = document.createElement('i');
            icon.classList.add('sort-icon', 'fas');

            icon.classList.add(sortState.direction === 'asc' ? 'fa-sort-up' : 'fa-sort-down');

            th.appendChild(icon);
        }
    });
}

function addSortListeners() {
    const headerCells = document.querySelectorAll('.data-table thead th');

    headerCells.forEach(th => {
        const columnInfo = sortableColumns.find(col => col.headerText === th.textContent);

        if (columnInfo) {
            th.classList.add('sortable');

            th.addEventListener('click', () => {
                sortTable(columnInfo.property, columnInfo.type);
            });
        }
    });
}

document.addEventListener("DOMContentLoaded", loadUsers);