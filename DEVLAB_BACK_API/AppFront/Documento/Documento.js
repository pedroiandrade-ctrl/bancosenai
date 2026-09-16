const URL_API = "http://localhost:7081/api/v1/documento";

async function enviarDocumento() {
   

    const codigoCliente = document.getElementById("codigoCliente").value;
    const inputArquivo = document.getElementById("arquivo");
    const arquivo = inputArquivo.files[0];  

    if (!codigoCliente || !arquivo) {
        alert("Por favor, preencha todos os campos.");
        return;
    }

    const dadosArquivo = new FormData();
    dadosArquivo.append("arquivo", arquivo);

    const response = await fetch(`${URL_API}/upload/${codigoCliente}`, {
        method: 'POST',
        body: dadosArquivo
    })

    if (response.ok) {
        alert("Documento enviado com sucesso!");
        document.getElementById("codigoCliente").value = "";
        document.getElementById("arquivo").value = "";
    }
    else {
        const erro = await response.json();
        alert(erro.message || "Erro ao enviar documento.");
    }
}