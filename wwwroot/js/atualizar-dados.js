function handleFieldChange() {
    const selectedField = document.getElementById('data-select').value;
    const passwordFields = document.getElementById('password-fields');
    const confirmPasswordField = document.getElementById('confirm-password-field');
    const newValueInput = document.getElementById('new-value');

    hideMessages();

    if (selectedField === 'senha') {
        passwordFields.classList.remove('hidden');
        confirmPasswordField.classList.remove('hidden');
        newValueInput.type = 'password';
        newValueInput.placeholder = 'Digite a nova senha';
    } else {
        passwordFields.classList.add('hidden');
        confirmPasswordField.classList.add('hidden');
        newValueInput.type = 'text';
        newValueInput.placeholder = 'Digite a nova informação';

        document.getElementById('current-password').value = '';
        document.getElementById('confirm-password').value = '';
    }
}

async function handleSubmit() {
    const id = document.getElementById('user-id').value.trim();
    const selectedField = document.getElementById('data-select').value;
    const newValue = document.getElementById('new-value').value.trim();

    hideMessages();

    if (!id) {
        showError("Por favor, informe o ID do usuário.");
        return;
    }

    if (!selectedField) {
        showError("Por favor, selecione um campo para alterar.");
        return;
    }

    if (!newValue) {
        showError("Por favor, digite a nova informação.");
        return;
    }

    let body = {};
    if (selectedField === "senha") {
        const currentPassword = document.getElementById('current-password').value.trim();
        const confirmPassword = document.getElementById('confirm-password').value.trim();

        if (!currentPassword) {
            showError('Por favor, digite sua senha atual.');
            return;
        }
        if (!confirmPassword) {
            showError('Por favor, confirme a nova senha.');
            return;
        }
        if (newValue !== confirmPassword) {
            showError('A confirmação da nova senha não confere com a nova senha digitada.');
            return;
        }

        body = {
            senhaAtual: currentPassword,
            novaSenha: newValue,
            confirmaNovaSenha: confirmPassword
        };
    } else {
        const map = {
            nome: "name",
            email: "email",
            telefone: "phoneNumber",
            data_nascimento: "dateOfBirth",
            nome_rua: "streetName",
            numero: "streetNumber",
            cidade: "city",
            estado: "state",
            pais: "country"
        };
        body[map[selectedField]] = newValue;
    }

    try {
        const response = await fetch(`http://localhost:5229/api/users/${id}`, {
            method: "PATCH",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(body)
        });

        if (!response.ok) {
            const err = await response.json();
            throw new Error(err.message || "Erro ao atualizar usuário.");
        }

        showSuccess();
        clearForm();
    } catch (err) {
        showError(err.message);
    }
}

//Mensagens de feedback
function showSuccess() {
    const successMessage = document.getElementById('success-message');
    successMessage.classList.remove('hidden');
    setTimeout(() => successMessage.classList.add('hidden'), 3000);
}

function showError(message) {
    const errorMessage = document.getElementById('error-message');
    errorMessage.textContent = message;
    errorMessage.classList.remove('hidden');
    setTimeout(() => errorMessage.classList.add('hidden'), 5000);
}

function hideMessages() {
    document.getElementById('success-message').classList.add('hidden');
    document.getElementById('error-message').classList.add('hidden');
}

function clearForm() {
    document.getElementById('user-id').value = '';
    document.getElementById('data-select').value = '';
    document.getElementById('new-value').value = '';
    document.getElementById('current-password').value = '';
    document.getElementById('confirm-password').value = '';
    document.getElementById('password-fields').classList.add('hidden');
    document.getElementById('confirm-password-field').classList.add('hidden');

    const newValueInput = document.getElementById('new-value');
    newValueInput.type = 'text';
    newValueInput.placeholder = 'Digite a nova informação';
}
