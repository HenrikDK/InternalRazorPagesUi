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

function breakoutClick(e, row){
    window.location = window.location + '' + row.getData().id;
}

var breakoutRowMenu = [
    {
        label:"<i class='fa fa-plus'></i> New",
        action:function(e, row){
            window.location = window.location + 'new';
        }
    },
    {
        label:"<i class='fa fa-clone'></i> Copy",
        action:function(e, row){
            window.location = window.location + 'new?copyFrom=' + row.getData().id;
        }
    },
    {
        separator:true,
    },
    {
        label:"<i class='fa fa-trash-o'></i> Delete",
        action:function(e, row){
            window.location = window.location + '' + row.getData().id + '?handler=delete';
        }
    },
]

var inlineRowMenu = [
    {
        label:"<i class='fa fa-plus'></i> New",
        action:function(e, row){
            row.getTable().addRow();
        }
    },
    {
        label:"<i class='fa fa-clone'></i> Copy",
        action:function(e, row){
            var existing = row.getData();
            var newRow = {};
            for (const property in existing) {
                if (property != "id"){
                    newRow[property] = existing[property]
                }
            }
            
            row.getTable().addRow(newRow)
        }
    },
    {
        separator:true,
    },
    {
        label:"<i class='fa fa-trash-o'></i> Delete",
        action:function(e, row){
            row.delete();
        }
    },
]

function selectClick(e, cell)
{
    cell.getRow().toggleSelect();
}

var headerMenu = function(){
    var menu = [];
    var columns = this.getColumns();

    for(let column of columns){
        if (!column.getDefinition().title){
            continue;
        }
        //create checkbox element using font awesome icons
        let icon = document.createElement("i");
        icon.classList.add("fa");
        icon.classList.add(column.isVisible() ? "fa-check-square" : "fa-square");

        //build label
        let label = document.createElement("span");
        let title = document.createElement("span");

        title.textContent = " " + column.getDefinition().title;

        label.appendChild(icon);
        label.appendChild(title);

        //create menu item
        menu.push({
            label:label,
            action:function(e){
                //prevent menu closing
                e.stopPropagation();

                //toggle current column visibility
                column.toggle();

                //change menu item icon
                if(column.isVisible()){
                    icon.classList.remove("fa-square");
                    icon.classList.add("fa-check-square");
                }else{
                    icon.classList.remove("fa-check-square");
                    icon.classList.add("fa-square");
                }
            }
        });
    }

    return menu;
};
