# InternalRazorPagesUi
.Net Razor Pages is extremely efficient for building internal web applications that don't require heavy use of javascript for high levels of interactivity.

These applications can typically rely on simple serverside rendering to get results out quickly while remaining open for extension by adding custom javascript and publicly available components as the application matures.

This application is intended as a simple template for building internal applications.

## Features
- ReverseProxy page to transparently integrate other UI MicroServices into main site.
- Default 404 page
- Minimal razor pages webapp using lamar DI

### Tabulator 4.9.3
- Persistent tables with column selection, re-ordering, sort direction.
- Multiple tables on a single page
- Pluggable persistent search control filtering multiple tables on a single page 
- Inline editing of data in a table
- Breakout editing of data from a table
- Persistent filtering of table contents across requests based on multi-select table header controller

### Bootstrap 5
- SideBar and NavBar implemented using HTML Grid based layout.
- Persistent SideBar state across requests using localStorage
- NavBar highlighting based on URL
- Placeholder for User Login
- Breakout editor using different bootstrap controls

### Vue 3
- Combining Vue mixins with razor pages to increase interactivity
- Combining Multiple .cshtml Vue Single File Components (SFC) with razor pages to increase interactivity.
