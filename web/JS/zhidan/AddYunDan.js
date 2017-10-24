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
    Ext.define('MainView', {
        extend: 'Ext.container.Viewport',

        layout: {
            align: 'center',
            type: 'vbox'
        },

        initComponent: function () {
            var me = this;

            Ext.applyIf(me, {
                items: [
                    {
                        xtype: 'panel',
                        flex: 1,
                        width: 471,
                        layout: {
                            type: 'column'
                        },
                        border:0,
                        items: [
                            {
                                xtype: 'combobox',
                                columnWidth: 0.65,
                                padding: 20,
                                valueField: 'ID',
                                displayField: 'MC',
                                queryMode: 'local',
                                store: province,
                                id: 'QiShiZhan_Province',
                                fieldLabel: '出发地',
                                listeners: {
                                    change: function (data, newValue, oldValue, eOpts) {
                                        city.loadData(newcity[newValue]);
                                    }
                                }
                            },
                            {
                                xtype: 'combobox',
                                columnWidth: 0.35,
                                padding: 20,
                                valueField: 'ID',
                                displayField: 'MC',
                                queryMode: 'local',
                                store: city,
                                id: 'QiShiZhan_City'
                            },
                            {
                                xtype: 'combobox',
                                columnWidth: 0.65,
                                padding: 20,
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
                                columnWidth: 0.35,
                                padding: 20,
                                valueField: 'ID',
                                displayField: 'MC',
                                queryMode: 'local',
                                store: city2,
                                id: 'DaoDaZhan_City'
                            },
                            {
                                xtype: 'textfield',
                                columnWidth: 0.8,
                                padding: 20,
                                fieldLabel: '公司'
                            },
                            {
                                xtype: 'button',
                                margin: '20 0 20 10',
                                iconCls:'add',
                                text: '选择'
                            },
                            {
                                xtype: 'textfield',
                                columnWidth: 1,
                                padding: 20,
                                fieldLabel: '单号'
                            },
                            {
                                xtype: 'textfield',
                                columnWidth: 1,
                                padding: 20,
                                fieldLabel: '扫描码'
                            },
                            {
                                xtype: 'label',
                                columnWidth: 1,
                                padding: 20,
                                html: '<a href="javascript:void(0);" onclick="Show();" style="float:left;">请先阅读相关协议</a>'
                            }
                        ],
                        buttonAlign: 'center',
                        buttons: [
                            {
                                text: '制单',
                                iconCls: 'enable',
                                handler: function () {
                                    alert(1);
                                }
                            },
                            {
                                text: '返回',
                                iconCls: 'back',
                                handler: function () {
                                    FrameStack.popFrame();
                                }
                            }
                        ]
                    }
                ]
            });

            me.callParent(arguments);
        }

    });

    new MainView();

    cityBind();

});

function cityBind() {
    var provincesData = [];
    for (var i = 0; i < cityList.provinces.length; i++) {
        var obj = {};
        obj.ID = cityList.provinces[i].name;
        obj.MC = cityList.provinces[i].name;
        provincesData.push(obj);

        var cityData = [];
        for (var j = 0; j < cityList.provinces[i].citys.length; j++) {
            var obj2 = {};
            obj2.ID = cityList.provinces[i].citys[j];
            obj2.MC = cityList.provinces[i].citys[j];
            cityData.push(obj2);
        }
        newcity[cityList.provinces[i].name] = cityData;
    }
    province.loadData(provincesData);
}

function Show() {
    alert(1);
}