$("#btnLoadMore").on("click", () => {
    let serviceCount = $("#services").children().length;
    console.log("Ok");
    fetch(`/Home/LoadMore?skip=${serviceCount}&take=8`, {
        method: 'GET'
    })
        .then(res => res.text())
        .then(data => {
            $("#services").append(data);
            serviceCount = $("#services").children().length;
            if (serviceCount >= 8) {
                $("#btnLoadMore").remove();
            }
        })
        .catch(error => console.log(error));
});