Ext.define('SFW4.Store', {
    extend: 'Ext.data.Store',
    alias: 'store.SFW4Store',
    constructor: function(option) {
        var data = option.data;
        var pageSize = option.pageSize;
        var total = option.total;
        var currentPage = option.currentPage;
        var onPageChange = option.onPageChange;
        var fields = option.fields;
        this.callParent([{
            fields: fields,
            storeId: option.storeId,
            sorters: option.sorters, //[{property: 'b', direction: 'DESC'}],
            data: {
                items: data,
                total: total
            },
            remoteSort: true,
            pageSize: option.pageSize,
            currentPage: currentPage,
            proxy: {
                type: 'memory',
                reader: {
                    type: 'json',
                    root: 'items',
                    totalProperty: 'total'
                }
}}]);
                this.setData = function(option) {
                    var me = this;
                    var data = option.data;
                    var pageSize = option.pageSize;
                    var total = option.total;
                    var currentPage = option.currentPage;
                    var sorters = option.sorters;
                    this.loadData(data);
                    this.pageSize = pageSize;
                    this.currentPage = currentPage;
                    this.totalCount = total;
                    //this.sorters = sorters;
                    this.sorters.clear();
                    this.sort(sorters, null, false, false);
                    this.fireEvent("load", this);
                    var tSorters = this.sorters;
                    //if (tSorters && tSorters.length) {
                    first = tSorters.first();
                    //if (first) { //gridpanel
                    Ext.ComponentManager.each(function(id, cmp) {
                        if (cmp.xtype && cmp.xtype == 'gridpanel') {
                            if (cmp.store == me) {
                                if (first) {
                                    hd = cmp.down('gridcolumn[dataIndex=' + first.property + ']');
                                    if (hd) {
                                        hd.setSortState(first.direction, false, true);
                                    }
                                }
                                else {
                                    Ext.Array.each(cmp.query('gridcolumn'), function() {
                                        this.setSortState(null, false, true);
                                    });
                                }
                            }
                        }
                    });
                    //}
                    //}
                }
                if (Ext.isFunction(option.onPageChange))
                    this.on("beforeload", function() {
                        option.onPageChange(this, arguments[1].page, this.getSorters());
                        return false;
                    });
            }
        });
    function createSFW4Store(option) {
        return sto = Ext.create('SFW4.Store', option);
    }


    Ext.define('SFW4.AutoSelectCellEditing', {
        extend: 'Ext.grid.plugin.CellEditing',
        constructor: function(option) {
            this.callParent([option]);
        },
        startEdit: function(record, columnHeader) {
            var ed = this.getEditor(record, columnHeader);
            ed.field.selectOnFocus = true;
            this.callParent([record, columnHeader]);
        }
    });     