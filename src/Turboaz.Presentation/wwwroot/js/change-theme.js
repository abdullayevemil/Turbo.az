function switchTheme() {
    event.preventDefault();

    let head = document.getElementsByTagName('head');

    let link = document.getElementById('custom-styles');

    if (link.href.includes('dark'))
    {
        let newLink = link.href.replace("dark", "light");

        link.href = newLink;

        localStorage.setItem("theme", "light");
    }
    else
    {
        let newLink = link.href.replace("light", "dark");

        link.href = newLink;

        localStorage.setItem("theme", "dark");
    }
};

window.addEventListener("load", (event) => {
    let head = document.getElementsByTagName('head');

    let link = document.getElementById('custom-styles');

    if (link.href.includes('dark'))
    {
        let newLink = link.href.replace("dark", "light");

        link.href = newLink;

        localStorage.setItem("theme", "light");
    }
    else
    {
        let newLink = link.href.replace("light", "dark");

        link.href = newLink;

        localStorage.setItem("theme", "dark");
    }
});