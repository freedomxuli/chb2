var newcity = {};

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

var city2 = Ext.create('Ext.data.Store', {
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
                                text: '出发地'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'DaoDaZhan',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '目的地'
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
                                        fieldLabel: '出发地',
                                        listeners: {
                                            change: function(data,newValue, oldValue, eOpts) {
                                                city.loadData(newcity[newValue]);
                                            }
                                        }
                                    },
                                    {
                                        xtype: 'combobox',
                                        width: 100,
                                        valueField: 'ID',
                                        displayField: 'MC',
                                        queryMode: 'local',
                                        store: city,
                                        id: 'QiShiZhan_City'
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
                                        fieldLabel: '目的地',
                                        listeners: {
                                            change: function (data, newValue, oldValue, eOpts) {
                                                city2.loadData(newcity[newValue]);
                                            }
                                        }
                                    },
                                    {
                                        xtype: 'combobox',
                                        width: 100,
                                        valueField: 'ID',
                                        displayField: 'MC',
                                        queryMode: 'local',
                                        store: city2,
                                        id: 'DaoDaZhan_City'
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
                                        text: '查询'
                                    },
                                    {
                                        xtype: 'button',
                                        iconCls: 'add',
                                        text: '新增运单',
                                        handler: function () {
                                            FrameStack.pushFrame({
                                                url: "AddYunDan.html",
                                                onClose: function (ret) {
                                                    DataBind();
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

    DataBind();
});

function cityBind() {
    var provincesData = [];
    for (var i = 0; i < cityList.provinces.length; i++)
    {
        var obj = {};
        obj.ID = cityList.provinces[i].name;
        obj.MC = cityList.provinces[i].name;
        provincesData.push(obj);

        var cityData = [];
        for (var j = 0; j < cityList.provinces[i].citys.length; j++)
        {
            var obj2 = {};
            obj2.ID = cityList.provinces[i].citys[j];
            obj2.MC = cityList.provinces[i].citys[j];
            cityData.push(obj2);
        }
        newcity[cityList.provinces[i].name] = cityData;
    }
    province.loadData(provincesData);
}

function DataBind() {
    
}
