class Sidebar {
    initToggleButton = () =>  {
        const sidebarBtn = document.querySelector('#sidebarCollapse');

        if (sidebarBtn) {
            sidebarBtn.addEventListener('click', function () {
                document.querySelector('#sidebar')?.classList.toggle('active');
            });
        }
    }
}

window['sidebar'] = new Sidebar();