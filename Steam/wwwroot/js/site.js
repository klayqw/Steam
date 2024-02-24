async function showGameDetails() {
    fetch('', { method: "GET" })
}

async function Back() {
    console.log(1)
    await fetch('/', { method: "GET" })
        .then(data => {
          window.location.href = '/';
    })
}