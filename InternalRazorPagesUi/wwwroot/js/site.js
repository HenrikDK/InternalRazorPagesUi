var mixinArray = [];

$( document ).ready(function() {
    updateNavBar();
    restoreSidebarMenu();
    getTabulators();
    restoreTableSearch();
    restoreSelectFilter();
    restoreShowAllControl();
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
    window.location = window.location + '/' + row.getData().id;
}

var breakoutRowMenu = [
    {
        label:"<i class='fa fa-plus'></i> New",
        action:function(e, row){
            window.location = window.location + '/new';
        }
    },
    {
        label:"<i class='fa fa-clone'></i> Copy",
        action:function(e, row){
            window.location = window.location + '/new?copyFrom=' + row.getData().id;
        }
    },
    {
        separator:true,
    },
    {
        label:"<i class='fa fa-trash-o'></i> Delete",
        action:function(e, row){
            window.location = window.location + '/' + row.getData().id + '?handler=delete';
        }
    },
]

function inlineRowMenu(component, e){
    //component - column/cell/row component that triggered the menu
    //e - click event object
    
    var menu = [
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
    ]
    
    if(!component.getData().isDelete){
        menu.push({
            label: "<i class='fa fa-trash-o'></i> Delete",
            action: function(e, row){
            var data = row.getData();
            data.isDelete = true;
            row.update(data);}
        });
    }
    else
    {
        menu.push({
            label:"<i class='fa fa-recycle'></i> Restore",
            action:function(e, row){
                var data = row.getData();
                data.isDelete = false;
                row.update(data);}
        });
    }

    return menu;
}

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
        window.localStorage.setItem('tabulator-' + tables[0].element.id + '-search', search)
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
        var search = window.localStorage.getItem('tabulator-' + tables[0].element.id + '-search');
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

function restoreShowAllControl()
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
    
    if (!data.isDelete && row.getElement().classList.contains("text-muted")){
        row.getElement().classList.remove("text-muted")
    }
}

function persistSelectFilters(filters, rows){
    var columns = this.modules.filter.headerFilterColumns;
    var filters = columns.map(function (x) {
        if (x.component){
            var values = x.component.getHeaderFilterValue();
            if (values.length > 0){
                var filters = values.split(',').map(h=> h.trim());
                return {field: x.field, filter: filters}
            }
            return {field: x.field, filter:[]}
        }
    });
    window.localStorage.setItem('tabulator-' + this.element.id + '-select-filters', JSON.stringify(filters));
    return true;
}

function restoreSelectFilter(){
    tables.forEach(t => {
        var filters = JSON.parse(window.localStorage.getItem('tabulator-' + t.element.id + '-select-filters'));
        var columns = t.modules.filter.headerFilterColumns;
        if (!filters) return;
        filters.forEach(f => {
            if (f.filter.length < 1){
                return;
            }
            columns.forEach(c => {
                if (c.field === f.field){
                    c.component.setHeaderFilterValue(f.filter);
                }
            })
        })
    });
}