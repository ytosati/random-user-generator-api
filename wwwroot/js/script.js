// Função para navegação entre páginas
function navigateTo(page) {
    window.location.href = page;
}

// Adiciona feedback visual quando uma página não existe ainda
document.addEventListener("DOMContentLoaded", function() {
    const buttons = document.querySelectorAll(".action-button");
    
    buttons.forEach(button => {
        button.addEventListener("click", function(e) {
            const href = this.getAttribute("onclick").match(/\'(.*?)\'/)[1];
            
            checkPageExists(href).then(exists => {
                if (!exists) {
                    // Mostra uma mensagem temporária se a página não existir
                    showTemporaryMessage("Página em desenvolvimento...");
                    e.preventDefault();
                    return false;
                }
            });
        });
    });
});

// Função para verificar se as páginas de destino existem
function checkPageExists(url) {
    return fetch(url, { method: "HEAD" })
        .then(response => response.ok)
        .catch(() => false);
}

// Função para mostrar mensagens temporárias
function showTemporaryMessage(message) {
    const messageDiv = document.createElement("div");
    messageDiv.textContent = message;
    messageDiv.style.cssText = `
        position: fixed;
        top: 20px;
        right: 20px;
        background: #667eea;
        color: white;
        padding: 1rem 2rem;
        border-radius: 10px;
        box-shadow: 0 10px 20px rgba(0, 0, 0, 0.2);
        z-index: 1000;
        font-weight: 600;
    `;
    
    document.body.appendChild(messageDiv);
    
    setTimeout(() => {
        messageDiv.remove();
    }, 2000);
}
