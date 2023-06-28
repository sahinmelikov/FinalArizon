let dbServiceCount = $("#dbServiceCount").val()

$("#btnLoadMore").on("click", () => {
    let serviceCount = $("#more").children().length
    console.log("Ok")
    fetch("/Services/LoadMore",{
        method: 'GET',
        body:
            JSON.stringify({
                skip: serviceCount
            })
    })
        .then(res => res.text())
        .then(data => {
            $("#services").append(data)
        })
    $.ajax("/Home/LoadMore", {

        method: "GET",
        data: {
            skip: serviceCount,
            take: 1
        },
        success: (data) => {
            $("#services").append(data)
            serviceCount = $("#services").children().length
            if (serviceCount >= dbServiceCount) {
                $("#btnLoadMore").remove()
            }
        }
    })
})
//btnLoadMore.addEventListener("click", function () {

//})
