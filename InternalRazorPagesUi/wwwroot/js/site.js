$( document ).ready(function() {
    updateNavBar();
    restoreSidebarMenu();
    getTabulators();
    restoreTableSearch();
    setupShowAllControl();
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

var tables = [];

function getTabulators() 
{
    for ( var i in window ) {
        if (window[i] instanceof Tabulator){
            tables.push(window[i]);
        }
    }
}

function searchTables(event){
    var search = document.getElementById("tableSearch").value;
    if (tables[0])
    {
        window.localStorage.setItem(tables[0].element.id + '-search', search)
    }

    if (search.length === 0){
        clearSearch();
        return;
    }
    
    tables.forEach(x => x.setFilter(matchAll, { value: search }))
}

function clearSearch(){
    tables.forEach(x => x.clearFilter())
}

function restoreTableSearch(){
    var searchBox = document.getElementById("tableSearch");
    if (searchBox){
        var search = window.localStorage.getItem(tables[0].element.id + '-search');
        if (search && search.length > 0){
            searchBox.value = search;
            tables.forEach(x => x.setFilter(matchAll, { value: search }))
        }
        searchBox.addEventListener("search", searchTables);
    }
}

function matchAll(data, filterParams) {
    //data - the data for the row being filtered
    //filterParams - params object passed to the filter
    //RegExp - modifier "i" - case insensitve

    var terms = filterParams.value.split(' ').map(x => RegExp("\\b" + x + "\\b",'i'))
    
    var matches = terms.map(r => {
        var match = false;
        for (var key in data) {
            if (r.test(data[key]) == true) {
                match = true;
            }
        }
        return match;
    })
    
    return !matches.includes(false);
}

var showAll = false;

function setupShowAllControl()
{
    var control = document.getElementById("show-all-check");
    if (control){
        var checked = window.localStorage.getItem(window.location + '-show-all');
        showAll = checked === "true";
        control.checked = showAll;
        control.addEventListener("change", showAllChange);
    }
}

function showAllChange()
{
    showAll = document.getElementById("show-all-check").checked
    window.localStorage.setItem(window.location + '-show-all', showAll);
    tables.forEach(x => x.redraw(true));
}

function softDeleteFormatter(row)
{
    var data = row.getData();
    
    if(data.isDelete && showAll)
    {
        row.getElement().classList.add("text-muted")
        row.getElement().hidden = false
    } 
    else if (data.isDelete){
        row.getElement().hidden = true
    }
}