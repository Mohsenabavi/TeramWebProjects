var overrideEditUrl = "",
    overrideDeleteUrl = "",
    overrideDetailUrl = "",
    deleteFileUrl = "",
    editFunction,
    initFunction,
    afterSuccess,
    editInSamePage = false;

var table, onAfterDataSave, changeDatatableOptions, onSuccessOverride, specialClearForm, initializeDatatable, reloadDataTable;

var routes = [
    '/',
    '/home',
    '/about',
    '/contact',
];