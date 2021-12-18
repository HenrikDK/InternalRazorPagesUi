$( document ).ready(function() {
    updateNavBar();
    restoreSidebarMenu();
});

function updateNavBar() {
    var url = window.location;
    $('a.nav-link').filter(function() {
        return this.href == url && !this.classList.contains("dropdown-toggle");
    }).addClass('active');
};

function persistSideBarState() {
    var menuItems = $('.menu-toggle').map(function () {
        return { 'target': $(this).data('bs-target'), 'aria': this.ariaExpanded}
    }).get();
    
    window.localStorage.setItem('side-bar-expand-state', JSON.stringify(menuItems));
}

function restoreSidebarMenu() {
    var menuItems = JSON.parse(window.localStorage.getItem('side-bar-expand-state'))

    $('.menu-toggle').each(function (index) {
        if ($(this).data('bs-target') === menuItems[index]['target'] && this.ariaExpanded !== menuItems[index]['aria'])
        {
            this.click()
        }
    });
}