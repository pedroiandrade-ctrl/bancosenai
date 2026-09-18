// Ajuste a URL para a rota base da sua API (remova o Swagger)
const URL_API = 'https://localhost:7081';

async function enviarDocumento() {
    const codigoCliente = document.getElementById("codigoCliente").value;
    const inputArquivo = document.getElementById("arquivo");
    const arquivo = inputArquivo.files[0];

    if (!codigoCliente || !arquivo) {
        alert("Informe o código do cliente e selecione um arquivo");
        return;
    }

    const dadosArquivo = new FormData();
    dadosArquivo.append("arquivo", arquivo);

    try {
        const response = await fetch(`${URL_API}/upload/${codigoCliente}`, {
            method: "POST",
            body: dadosArquivo
        });

        if (response.ok) {
            document.getElementById("codigoCliente").value = "";
            document.getElementById("arquivo").value = "";
            alert("Documento enviado com sucesso!");
        } else {
            const erro = await response.json();
            alert("Erro: " + (erro.message || "Falha ao enviar o documento"));
        }
    } catch (error) {
        console.error("Erro na requisição:", error);
        alert("Erro ao conectar com o servidor.");
    }
}

async function listarDocumentos() {
    const codigoCliente = document.getElementById("codigoClienteBusca").value;

    if (!codigoCliente) {
        alert("Informe o código do cliente para buscar");
        return;
    }

    try {
        const response = await fetch(`${URL_API}/api/v1/Documento/cliente/${codigoCliente}`);

        if (response.ok) {
            const documentos = await response.json();
            const corpoTabela = document.getElementById("corpoTabela");
            corpoTabela.innerHTML = ""; 

            documentos.forEach(doc => {
                const linha = document.createElement("tr");
                linha.innerHTML = `
    <td>${doc.id}</td>
    <td>${doc.nome}</td>
    <td>${doc.extensao}</td>
    <td>
        <button class="btn-baixar" style="background-color: #ffc107;" onclick="baixarDocumento(${doc.id}, '${doc.nome}')">Baixar</button>
        <button class="btn-excluir">Excluir</button>
    </td>
`;

                corpoTabela.appendChild(linha);
            });
        } else {
            alert("Erro ao buscar documentos.");
        }
    } catch (error) {
        console.error("Erro na requisição:", error);
        alert("Erro ao conectar com o servidor.");
    }
}

async function baixarDocumento(id, nomeArquivo) {
    try {
        const response = await fetch(`${URL_API}/api/v1/Documento/download/${id}`);

        if (response.ok) {
            const blob = await response.blob();
            const url = window.URL.createObjectURL(blob);

            const a = document.createElement('a');
            a.href = url;
            a.download = nomeArquivo; 
            document.body.appendChild(a);
            a.click(); 
            a.remove();
            window.URL.revokeObjectURL(url);
        } else {
            alert("Erro ao realizar o download do arquivo.");
        }
    } catch (error) {
        console.error("Erro na requisição de download:", error);
        alert("Erro ao conectar com o servidor.");
    }
}