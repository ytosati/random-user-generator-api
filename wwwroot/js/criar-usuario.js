async function createUser() {
    console.log("Botão clicado → chamando API...");

    try {
        const response = await fetch("http://localhost:5229/api/users/generate", {
            method: "POST",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
            }
        });

        if (!response.ok) {
            throw new Error("Erro ao gerar usuário. Status: " + response.status);
        }

        const user = await response.json();

        console.log("Usuário criado:", user);

        const tableRows = document.querySelectorAll(".data-table tbody tr");

        tableRows[0].children[1].textContent = user.id;
        tableRows[1].children[1].textContent = user.name;
        tableRows[2].children[1].textContent = user.email;
        tableRows[3].children[1].textContent = user.password;
        tableRows[4].children[1].textContent = user.phoneNumber;
        tableRows[5].children[1].textContent = user.dateOfBirth;
        tableRows[6].children[1].textContent = user.streetName;
        tableRows[7].children[1].textContent = user.streetNumber;
        tableRows[8].children[1].textContent = user.city;
        tableRows[9].children[1].textContent = user.state;
        tableRows[10].children[1].textContent = user.country;

        const successDiv = document.getElementById("success-message");
        successDiv.classList.remove("hidden");

        setTimeout(() => {
            successDiv.classList.add("hidden");
        }, 3000);

    } catch (err) {
        alert("Erro: " + err.message);
        console.error(err);
    }
}

window.createUser = createUser;
