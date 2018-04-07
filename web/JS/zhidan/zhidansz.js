var btStore = Ext.create('Ext.data.Store', {
    fields: [
        'DingDanSetListMC', 'DingDanSetListBS', 'DingDanSetListPX', 'DingDanSetListID'
    ]
});

var xtStore = Ext.create('Ext.data.Store', {
    fields: [
        'DingDanSetListMC', 'DingDanSetListBS', 'DingDanSetListPX', 'DingDanSetListID'
    ]
});

var SelectionStore = Ext.create('Ext.data.Store', {
    fields: [
        'DingDanSetSelectionID', 'DingDanSetSelectionMC'
    ]
});

Ext.onReady(function () {
    Ext.define('MainView', {
        extend: 'Ext.container.Viewport',

        layout: {
            type: 'fit'
        },

        initComponent: function () {
            var me = this;

            Ext.applyIf(me, {
                items: [
                    {
                        xtype: 'panel',
                        items: [
                            {
                                xtype: 'checkboxfield',
                                width: 300,
                                labelWidth: 200,
                                id:'SetModel',
                                fieldLabel: '是否启用个性化定义',
                                listeners: {
                                    change: function (data, value) {
                                        CS('CZCLZ.Handler.ChangeSetModel', function (retVal) {
                                            if (retVal) {

                                            }
                                        }, CS.onError, value);
                                    }
                                }
                            },
                            {
                                xtype: 'gridpanel',
                                title: '必填区',
                                store: btStore,
                                border: 1,
                                columnLines: 1,
                                height: document.documentElement.clientHeight / 2,
                                scroll: true,
                                columns: [
                                    {
                                        xtype: 'gridcolumn',
                                        dataIndex: 'DingDanSetListMC',
                                        sortable: false,
                                        menuDisabled: true,
                                        flex: 1,
                                        text: '名称'
                                    },
                                    {
                                        xtype: 'gridcolumn',
                                        dataIndex: 'DingDanSetListID',
                                        sortable: false,
                                        menuDisabled: true,
                                        flex: 1,
                                        text: '操作',
                                        renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                                            if (value)
                                                return "<a href='javascript:void(0);' onclick='Del(\"" + value + "\");'>删除</a>";
                                        }
                                    }
                                ],
                                dockedItems: [
                                    {
                                        xtype: 'toolbar',
                                        dock: 'top',
                                        items: [
                                            {
                                                xtype: 'button',
                                                iconCls: 'application',
                                                text: '新增必填和选填选项',
                                                handler: function () {
                                                    var win = new SelectionWin({ lb: 0 });
                                                    win.show(null, function () {
                                                        GetSelection();
                                                    });
                                                }
                                            },
                                            {
                                                xtype: 'button',
                                                iconCls:'add',
                                                text: '自定义必填和选填选项',
                                                handler: function () {
                                                    var win = new AddSelection({ lb: 0 });
                                                    win.show(null, function () {

                                                    });
                                                }
                                            }
                                        ]
                                    }
                                ]
                            },
                            {
                                xtype: 'gridpanel',
                                title: '选填区',
                                store: xtStore,
                                border: 1,
                                columnLines: 1,
                                height: document.documentElement.clientHeight / 2 - 30,
                                scroll: true,
                                columns: [
                                    {
                                        xtype: 'gridcolumn',
                                        dataIndex: 'DingDanSetListMC',
                                        sortable: false,
                                        menuDisabled: true,
                                        flex: 1,
                                        text: '名称'
                                    },
                                    {
                                        xtype: 'gridcolumn',
                                        dataIndex: 'DingDanSetListID',
                                        sortable: false,
                                        menuDisabled: true,
                                        flex: 1,
                                        text: '操作',
                                        renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                                            return "<a href='javascript:void(0);' onclick='Del(\"" + value + "\");'>删除</a>";
                                        }
                                    }
                                ],
                                dockedItems: [
                                    {
                                        xtype: 'toolbar',
                                        dock: 'top',
                                        items: [
                                            {
                                                xtype: 'button',
                                                iconCls: 'application',
                                                text: '新增必填和选填选项',
                                                handler: function () {
                                                    var win = new SelectionWin({ lb: 1 });
                                                    win.show(null, function () {
                                                        GetSelection();
                                                    });
                                                }
                                            },
                                            {
                                                xtype: 'button',
                                                iconCls: 'add',
                                                text: '自定义必填和选填选项',
                                                handler: function () {
                                                    var win = new AddSelection({ lb: 1 });
                                                    win.show(null, function () {

                                                    });
                                                }
                                            }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }
                ]
            });

            me.callParent(arguments);
        }

    });

    new MainView();

    dataBind();

});

function dataBind()
{
    CS('CZCLZ.Handler.GetSetModel', function (retVal) {
        if (retVal)
        {
            if (parseInt(retVal.modelnum) > 0)
                Ext.getCmp("SetModel").setValue(true);
            else
                Ext.getCmp("SetModel").setValue(false);
            btStore.loadData(retVal.dt_btstore);
            xtStore.loadData(retVal.dt_xtstore);
        }
    }, CS.onError);

    //CS('CZCLZ.Handler.GetSetModel', function (retVal) {
    //    if (retVal)
    //    {

    //    }
    //},CS.onError);
}

function Del(DingDanSetListID)
{
    CS('CZCLZ.Handler.DeleteDingDanSet', function (retVal) {
        if (retVal)
        {
            Ext.Msg.alert("提示", "保存成功！", function () {
                dataBind();
            });
        }
    }, CS.onError, DingDanSetListID)
}

Ext.define('SelectionWin', {
    extend: 'Ext.window.Window',

    height: 383,
    width: 327,
    layout: {
        type: 'fit'
    },
    title: '选项',
    modal: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    layout: {
                        type: 'fit'
                    },
                    items: [
                        {
                            xtype: 'gridpanel',
                            border: 1,
                            columnLines: 1,
                            store: SelectionStore,
                            columns: [
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'DingDanSetSelectionMC',
                                    sortable: false,
                                    menuDisabled: true,
                                    flex: 2,
                                    text: '名称'
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'DingDanSetSelectionID',
                                    sortable: false,
                                    menuDisabled: true,
                                    flex: 1,
                                    text: '操作',
                                    renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                                        return "<a href='javascript:void(0);' onclick='Sel(\"" + value + "\",\"" + me.lb + "\");'>选择</a>";
                                    }
                                }
                            ]
                        }
                    ],
                    buttonAlign: 'center',
                    buttons: [
                        {
                            text: '关闭',
                            iconCls:'close',
                            handler: function () {
                                me.close();
                            }
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }

});

function GetSelection() {
    CS('CZCLZ.Handler.GetSelection', function (retVal) {
        if (retVal) {
            SelectionStore.loadData(retVal);
        }
    }, CS.onError);
}

function Sel(DingDanSetSelectionID,lb)
{
    CS('CZCLZ.Handler.InsertModel', function (retVal) {
        if (retVal) {
            Ext.Msg.alert("提示", "添加成功！", function () {
                dataBind();
                GetSelection();
            });
        }
    }, CS.onError, DingDanSetSelectionID, lb);
}

Ext.define('AddSelection', {
    extend: 'Ext.window.Window',

    height: 175,
    width: 507,
    layout: {
        type: 'fit'
    },
    title: '新增选项',
    modal: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    layout: {
                        align: 'center',
                        type: 'vbox'
                    },
                    items: [
                        {
                            xtype: 'panel',
                            flex: 1,
                            width: 308,
                            border: 0,
                            items: [
                                {
                                    xtype: 'textfield',
                                    id: 'DingDanSetListMC',
                                    padding:'30 0 0 0',
                                    fieldLabel: '标签名称'
                                }
                            ]
                        }
                    ],
                    buttonAlign: 'center',
                    buttons: [
                        {
                            text: '保存',
                            iconCls:'save',
                            handler: function () {
                                CS('CZCLZ.Handler.InsertSelection', function (retVal) {
                                    if (retVal) {
                                        Ext.Msg.alert("提示", "保存成功！", function () {
                                            dataBind();
                                            me.close();
                                        });
                                    }
                                }, CS.onError, Ext.getCmp('DingDanSetListMC').getValue(), me.lb);
                            }
                        },
                        {
                            text: '关闭',
                            iconCls: 'close',
                            handler: function () {
                                me.close();
                            }
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }

});