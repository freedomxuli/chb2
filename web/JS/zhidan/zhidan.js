var pageSize = 15;

var ddStore = createSFW4Store({
    pageSize: pageSize,
    total: 1,
    currentPage: 1,
    fields: [
        'UserDenno', 'QiShiZhan', 'DaoDaZhan', 'SuoShuGongSi', 'BangDingTime', 'QiShiZhan_QX', 'DaoDaZhan_QX'
    ],
    onPageChange: function (sto, nPage, sorters) {
        DataBind(nPage);
    }
});

var newcity = {};

var newqx = {};

var province = Ext.create('Ext.data.Store', {
    fields: [
        'ID', 'MC'
    ]
});

var city = Ext.create('Ext.data.Store', {
    fields: [
        'ID', 'MC'
    ]
});

var qx = Ext.create('Ext.data.Store', {
    fields: [
        'ID', 'MC'
    ]
});

var city2 = Ext.create('Ext.data.Store', {
    fields: [
        'ID', 'MC'
    ]
});

var qx2 = Ext.create('Ext.data.Store', {
    fields: [
        'ID', 'MC'
    ]
});

Ext.onReady(function () {

    Ext.define('mainView', {
        extend: 'Ext.container.Viewport',

        layout: {
            type: 'fit'
        },

        initComponent: function () {
            var me = this;

            Ext.applyIf(me, {
                items: [
                    {
                        xtype: 'gridpanel',
                        columnLines: 1,
                        border: 1,
                        store:ddStore,
                        columns: [
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'UserDenno',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '单号'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'QiShiZhan',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '出发地',
                                renderer: function (value,cellmeta,record,rowIndex,columnIndex,store)
                                {
                                    if (record.data.QiShiZhan_QX != "" && record.data.QiShiZhan_QX != null)
                                        return value + " " + record.data.QiShiZhan_QX;
                                    else
                                        return value;
                                }
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'DaoDaZhan',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '目的地',
                                renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                                    if (record.data.DaoDaZhan_QX != "" && record.data.DaoDaZhan_QX != null)
                                        return value + " " + record.data.DaoDaZhan_QX;
                                    else
                                        return value;
                                }
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'SuoShuGongSi',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '公司'
                            },
                            {
                                xtype: 'datecolumn',
                                dataIndex: 'BangDingTime',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                format:'Y-m-d',
                                text: '绑定时间'
                            }
                        ],
                        dockedItems: [
                            {
                                xtype: 'toolbar',
                                dock: 'top',
                                items: [
                                    {
                                        xtype: 'combobox',
                                        labelWidth: 50,
                                        width: 140,
                                        valueField: 'ID',
                                        displayField: 'MC',
                                        queryMode: 'local',
                                        store:province,
                                        id: 'QiShiZhan_Province',
                                        fieldLabel: '起始站',
                                        listeners: {
                                            change: function(data,newValue, oldValue, eOpts) {
                                                city.loadData(newcity[newValue]);
                                            }
                                        }
                                    },
                                    {
                                        xtype: 'combobox',
                                        width: 90,
                                        padding: '0 10 10 0',
                                        valueField: 'ID',
                                        displayField: 'MC',
                                        queryMode: 'local',
                                        store: city,
                                        id: 'QiShiZhan_City',
                                        listeners: {
                                            change: function (data, newValue, oldValue, eOpts) {
                                                qx.loadData(newqx[newValue]);
                                            }
                                        }
                                    },
                                    {
                                        xtype: 'combobox',
                                        width: 90,
                                        padding: '0 10 10 0',
                                        valueField: 'ID',
                                        displayField: 'MC',
                                        queryMode: 'local',
                                        store: qx,
                                        id: 'QiShiZhan_Qx'
                                    },
                                    {
                                        xtype: 'combobox',
                                        labelWidth: 50,
                                        width: 140,
                                        valueField: 'ID',
                                        displayField: 'MC',
                                        queryMode: 'local',
                                        store: province,
                                        id: 'DaoDaZhan_Province',
                                        fieldLabel: '到达站',
                                        listeners: {
                                            change: function (data, newValue, oldValue, eOpts) {
                                                city2.loadData(newcity[newValue]);
                                            }
                                        }
                                    },
                                    {
                                        xtype: 'combobox',
                                        width: 90,
                                        padding: '0 10 10 0',
                                        valueField: 'ID',
                                        displayField: 'MC',
                                        queryMode: 'local',
                                        store: city2,
                                        id: 'DaoDaZhan_City',
                                        listeners: {
                                            change: function (data, newValue, oldValue, eOpts) {
                                                qx2.loadData(newqx[newValue]);
                                            }
                                        }

                                    },
                                    {
                                        xtype: 'combobox',
                                        width: 90,
                                        padding: '0 10 10 0',
                                        valueField: 'ID',
                                        displayField: 'MC',
                                        queryMode: 'local',
                                        store: qx2,
                                        id: 'DaoDaZhan_Qx'
                                    },
                                    {
                                        xtype: 'textfield',
                                        width: 130,
                                        labelWidth: 30,
                                        id: 'SuoShuGongSi',
                                        fieldLabel: '公司'
                                    },
                                    {
                                        xtype: 'textfield',
                                        width: 130,
                                        labelWidth: 30,
                                        id: 'UserDenno',
                                        fieldLabel: '单号'
                                    },
                                    {
                                        xtype: 'button',
                                        iconCls:'search',
                                        text: '查询',
                                        handler: function () {
                                            DataBind(1);
                                        }
                                    },
                                    {
                                        xtype: 'button',
                                        iconCls: 'add',
                                        text: '新增运单',
                                        handler: function () {
                                            FrameStack.pushFrame({
                                                url: "AddYunDan.html",
                                                onClose: function (ret) {
                                                    DataBind(1);
                                                }
                                            });
                                        }
                                    }
                                ]
                            },
                            {
                                xtype: 'pagingtoolbar',
                                dock: 'bottom',
                                width: 360,
                                store: ddStore,
                                displayInfo: true
                            }
                        ]
                    }
                ]
            });

            me.callParent(arguments);
        }

    });

    new mainView();

    cityBind();

    DataBind(1);
});

function cityBind() {
    var provincesData = [];
    for (var i = 0; i < cityData3.length; i++) {
        var obj = {};
        obj.ID = cityData3[i].text;
        obj.MC = cityData3[i].text;
        provincesData.push(obj);

        var cityData = [];
        for (var j = 0; j < cityData3[i].children.length; j++) {
            var obj2 = {};
            obj2.ID = cityData3[i].children[j].text;
            obj2.MC = cityData3[i].children[j].text;
            cityData.push(obj2);
            var qxData = [];
            for (var k = 0; k < cityData3[i].children[j].children.length; k++) {
                var obj3 = {};
                obj3.ID = cityData3[i].children[j].children[k].text;
                obj3.MC = cityData3[i].children[j].children[k].text;
                qxData.push(obj3);
            }

            newqx[cityData3[i].children[j].text] = qxData;
        }
        newcity[cityData3[i].text] = cityData;
    }
    province.loadData(provincesData);
}

function DataBind(cp) {
    CS('CZCLZ.Handler.GetZhiDanList', function (retVal) {
        if (retVal)
        {
            ddStore.setData({
                data: retVal.dt,
                pageSize: pageSize,
                total: retVal.ac,
                currentPage: retVal.cp
            });
        }
    }, CS.onError, cp, pageSize, Ext.getCmp('QiShiZhan_Province').getValue(), Ext.getCmp('QiShiZhan_City').getValue(), Ext.getCmp('QiShiZhan_Qx').getValue(), Ext.getCmp('DaoDaZhan_Province').getValue(), Ext.getCmp('DaoDaZhan_City').getValue(), Ext.getCmp('DaoDaZhan_Qx').getValue(), Ext.getCmp('SuoShuGongSi').getValue(), Ext.getCmp('UserDenno').getValue());
}
