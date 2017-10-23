var Up_userid = CZCLZUser.UserID;

//************************************数据源*****************************************
var typeStore = Ext.create('Ext.data.Store', {
    fields: [
        { name: 'UL_ID' },
       { name: 'lp_id' },
       { name: 'lp_name' },
       { name: 'lp_path' },
       { name: 'UL_PX' }
    ]
});


var lpstore = Ext.create('Ext.data.Store', {
    fields: [
        { name: 'lp_id' },
       { name: 'lp_name' },
       { name: 'lp_path' }
    ]
});

var cellEditing = Ext.create('Ext.grid.plugin.CellEditing', {
    clicksToEdit: 1,
    listeners: {
        beforeedit: function (a, b, c) {
           
        },
        validateedit: function (editor, e, op) {
            CS('CZCLZ.LPMag.Update', function (retVal) {
                if (retVal) {
                    typeStore.loadData(retVal);
                }
            }, CS.onError, e.record.data.UL_ID,e.value,Up_userid);
        }
    }
});

//************************************数据源*****************************************

//************************************页面方法***************************************
function getType() {
    CS('CZCLZ.LPMag.GetLPByUserid', function (retVal) {
        if (retVal) {
            typeStore.loadData(retVal);
        }

    }, CS.onError, Up_userid);
}

//************************************页面方法***************************************

//************************************弹出界面***************************************
Ext.define('addWin', {
    extend: 'Ext.window.Window',

    height: 350,
    width: 500,
    layout: {
        type: 'fit'
    },
    closeAction: 'destroy',
    modal: true,
    title: '新增首页设置',

    initComponent: function () {
        var me = this;
        me.items = [
            {
                xtype: 'gridpanel',
                store: lpstore,
                id:'lpgp',
                selModel: Ext.create('Ext.selection.CheckboxModel', {

                }),
                columns: [Ext.create('Ext.grid.RowNumberer'),
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'lp_id',
                             hidden: true,
                             sortable: false,
                             menuDisabled: true,
                             width: 100,
                             text: '首页ID'
                         },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'lp_name',
                            sortable: false,
                            menuDisabled: true,
                            width: 150,
                            text: '首页名称'
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'lp_path',
                            hidden:true,
                            sortable: false,
                            menuDisabled: true,
                            width: 250,
                            text: '首页地址'
                        }
                ],
                viewConfig: {

                },
                buttonAlign: 'center',
                buttons: [
                    {
                        text: '确定',
                        handler: function () {
                            var idlist = [];
                            var grid = Ext.getCmp("lpgp");
                            var rds = grid.getSelectionModel().getSelection();

                            if (rds.length == 0) {
                                Ext.Msg.show({
                                    title: '提示',
                                    msg: '请选择首页设置页面!',
                                    buttons: Ext.MessageBox.OK,
                                    icon: Ext.MessageBox.INFO
                                });
                                return;
                            }
                            var lpid = "";
                            for (i = 0; i < rds.length; i++) {
                                idlist.push(rds[i].data.lp_id);
                            }
                            var me = this;

                            CS('CZCLZ.LPMag.SaveLP', function (retVal) {
                                if (retVal) {
                                    typeStore.loadData(retVal);
                                }
                                me.up('window').close()
                            }, CS.onError, Up_userid, idlist);
                        }
                    },
                    {
                        text: '取消',
                        handler: function () {
                            this.up('window').close();
                        }
                    }
                ]
            }
        ];
        me.callParent(arguments);
    }
});

//************************************弹出界面***************************************

//************************************主界面*****************************************
    Ext.define('SBTypeSet', {
        extend: 'Ext.window.Window',

        layout: {
            type: 'fit'
        },
        height: 500,
        width: 650,
        title: '个性化首页设定',
        closeAction: 'destroy',
        modal: true,
        initComponent: function () {
            var me = this;
            me.items = [
                {
                    xtype: 'gridpanel',
                    id: 'JsGrid',
                    store: typeStore,
                    plugins: [cellEditing],
                    selModel: Ext.create('Ext.selection.CheckboxModel', {

                    }),
                    columns: [Ext.create('Ext.grid.RowNumberer'),
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'UL_ID',
                                hidden: true,
                                sortable: false,
                                menuDisabled: true,
                                width: 100,
                                text: '关联ID'
                            },
                             {
                                 xtype: 'gridcolumn',
                                 dataIndex: 'lp_id',
                                 hidden: true,
                                 sortable: false,
                                 menuDisabled: true,
                                 width: 100,
                                 text: '首页ID'
                             },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'lp_name',
                                sortable: false,
                                menuDisabled: true,
                                width: 250,
                                text: '首页名称'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'lp_path',
                                hidden:true,
                                sortable: false,
                                menuDisabled: true,
                                width: 200,
                                text: '首页地址'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'UL_PX',
                                sortable: false,
                                menuDisabled: true,
                                text: '排序',
                                field: {
                                    xtype: 'numberfield',
                                    allowBlank: false,
                                    selectOnFocus: true
                                }
                            }
                    ],
                    viewConfig: {

                    },
                    dockedItems: [
                                {
                                    xtype: 'toolbar',
                                    dock: 'top',
                                    items: [
                                        {
                                            xtype: 'buttongroup',
                                            title: '',
                                            items: [
                                                {
                                                    xtype: 'button',
                                                    iconCls: 'add',
                                                    text: '选择',
                                                    handler: function () {
                                                        var win = new addWin();
                                                        win.show(null, function () {
                                                            CS('CZCLZ.LPMag.GetLP', function (retVal) {
                                                                if (retVal) {
                                                                    lpstore.loadData(retVal);
                                                                }
                                                            }, CS.onError);
                                                        });
                                                    }
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'buttongroup',
                                            title: '',
                                            items: [
                                                {
                                                    xtype: 'button',
                                                    iconCls: 'delete',
                                                    text: '删除',
                                                    handler: function () {
                                                        var idlist = [];
                                                        var grid = Ext.getCmp("JsGrid");
                                                        var rds = grid.getSelectionModel().getSelection();
                                                        if (rds.length == 0) {
                                                            Ext.Msg.show({
                                                                title: '提示',
                                                                msg: '请选择至少一条要删除的记录!',
                                                                buttons: Ext.MessageBox.OK,
                                                                icon: Ext.MessageBox.INFO
                                                            });
                                                            return;
                                                        }

                                                        Ext.MessageBox.confirm("提示", "是否删除你所选?", function (obj) {
                                                            if (obj == "yes") {
                                                                for (var n = 0, len = rds.length; n < len; n++) {
                                                                    var rd = rds[n];

                                                                    idlist.push(rd.get("UL_ID"));
                                                                }

                                                                CS('CZCLZ.LPMag.Delete', function (retVal) {
                                                                    if (retVal) {
                                                                        typeStore.loadData(retVal);
                                                                    }
                                                                }, CS.onError, Up_userid,idlist);
                                                            }
                                                            else {
                                                                return;
                                                            }
                                                        });


                                                    }
                                                }
                                            ]
                                        }
                                    ]
                                }
                    ]
                }
            ];
            me.callParent(arguments);
        }
    });
//************************************主界面*****************************************
