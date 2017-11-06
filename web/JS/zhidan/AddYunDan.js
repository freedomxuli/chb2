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

var gsStore = Ext.create('Ext.data.Store', {
    fields: [
        'SuoShuGongSi'
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
                                xtype: 'textfield',
                                columnWidth: 1,
                                id: 'HiddenID',
                                fieldLabel: '隐藏设备号',
                                margin: '-25 0 0 0',
                                listeners: {
                                    'change': function (field, newValue, oldValue) {
                                        Ext.getCmp("GpsDeviceID").setValue(newValue);
                                    }
                                }
                            },
                            {
                                xtype: 'label',
                                columnWidth: 1,
                                html: "<div style='color:red;'>打开页面先扫描枪扫码，如果获取不到数据，请点击重置设备号再次尝试！</div>",
                            },
                            {
                                xtype: 'textfield',
                                columnWidth: 0.7,
                                padding: '0 20 20 20',
                                id: 'GpsDeviceID',
                                readOnly: true,
                                fieldLabel: '扫描码'
                            },
                            {
                                xtype: 'button',
                                iconCls: 'close',
                                text: '重置设备号',
                                columnWidth: 0.3,
                                margin: '0 20 0 0',
                                handler: function () {
                                    Ext.getCmp("GpsDeviceID").setValue("");
                                    Ext.getCmp("HiddenID").setValue("");
                                    Ext.getCmp('HiddenID').focus(true, true);
                                }
                            },
                            {
                                xtype: 'textfield',
                                columnWidth: 0.7,
                                padding: '0 20 20 20',
                                id: 'GpsDeviceIDByHand',
                                fieldLabel: '输入码'
                            },
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
                                id: 'SuoShuGongSi',
                                fieldLabel: '公司'
                            },
                            {
                                xtype: 'button',
                                margin: '20 0 20 10',
                                iconCls:'add',
                                text: '选择',
                                handler: function () {
                                    var win = new PickCompany();
                                    win.show(null, function () {
                                        CS('CZCLZ.Handler.GetCompanyHis', function (retVal) {
                                            if (retVal) {
                                                gsStore.loadData(retVal);
                                            }
                                        }, CS.onError);
                                    });
                                }
                            },
                            {
                                xtype: 'textfield',
                                columnWidth: 1,
                                padding: 20,
                                id: 'UserDenno',
                                fieldLabel: '单号'
                            },
                            {
                                xtype: 'textarea',
                                columnWidth: 1,
                                padding: 20,
                                id: 'YunDanRemark',
                                fieldLabel: '备注'
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
                                    if (Ext.getCmp("QiShiZhan_Province").getValue() == "" || Ext.getCmp("QiShiZhan_Province").getValue() == null)
                                    {
                                        Ext.Msg.alert("提示", "出发地省份为必填项！");
                                        return false;
                                    }
                                    if (Ext.getCmp("QiShiZhan_City").getValue() == "" || Ext.getCmp("QiShiZhan_City").getValue() == null) {
                                        Ext.Msg.alert("提示", "出发地城市为必填项！");
                                        return false;
                                    }
                                    if (Ext.getCmp("DaoDaZhan_Province").getValue() == "" || Ext.getCmp("DaoDaZhan_Province").getValue() == null) {
                                        Ext.Msg.alert("提示", "目的地省份为必填项！");
                                        return false;
                                    }
                                    if (Ext.getCmp("DaoDaZhan_City").getValue() == "" || Ext.getCmp("DaoDaZhan_City").getValue() == null) {
                                        Ext.Msg.alert("提示", "目的地城市为必填项！");
                                        return false;
                                    }
                                    if (Ext.getCmp("SuoShuGongSi").getValue() == "" || Ext.getCmp("SuoShuGongSi").getValue() == null) {
                                        Ext.Msg.alert("提示", "公司为必填项！");
                                        return false;
                                    }
                                    if (Ext.getCmp("UserDenno").getValue() == "" || Ext.getCmp("UserDenno").getValue() == null) {
                                        Ext.Msg.alert("提示", "单号为必填项！");
                                        return false;
                                    }
                                    if (Ext.getCmp("GpsDeviceIDByHand").getValue() != "" && Ext.getCmp("GpsDeviceIDByHand").getValue() != null && Ext.getCmp("GpsDeviceID").getValue() != "" && Ext.getCmp("GpsDeviceID").getValue() != null)
                                    {
                                        Ext.Msg.alert("提示", "扫描码和输入码不能同时存在！");
                                        return false;
                                    }
                                    if ((Ext.getCmp("GpsDeviceID").getValue() == "" || Ext.getCmp("GpsDeviceID").getValue() == null) && (Ext.getCmp("GpsDeviceIDByHand").getValue() == "" || Ext.getCmp("GpsDeviceIDByHand").getValue() == null)) {
                                        Ext.Msg.alert("提示", "扫描码或输入码必须填写一项！");
                                        return false;
                                    }
                                    if (Ext.getCmp("GpsDeviceIDByHand").getValue() != "" && Ext.getCmp("GpsDeviceIDByHand").getValue() != null)
                                    {
                                        CS('CZCLZ.Handler.IsBangBind', function (ret) {
                                            if (ret) {
                                                CS('CZCLZ.Handler.SaveYunDan', function (retVal) {
                                                    if (retVal) {
                                                        Ext.Msg.alert("提示", "制单成功！", function () {
                                                            FrameStack.popFrame();
                                                        });
                                                    } else {
                                                        Ext.Msg.alert("提示", "制单失败！");
                                                        return false;
                                                    }
                                                }, CS.onError, Ext.getCmp("QiShiZhan_Province").getValue(), Ext.getCmp("QiShiZhan_City").getValue(), Ext.getCmp("DaoDaZhan_Province").getValue(), Ext.getCmp("DaoDaZhan_City").getValue(), Ext.getCmp("SuoShuGongSi").getValue(), Ext.getCmp("UserDenno").getValue(), Ext.getCmp("GpsDeviceIDByHand").getValue(), Ext.getCmp("YunDanRemark").getValue());
                                            } else {
                                                Ext.Msg.alert("提示", "输入码必须先解除绑定再制单！");
                                                return false;
                                            }
                                        }, CS.onError, Ext.getCmp("GpsDeviceIDByHand").getValue());
                                    } else if (Ext.getCmp("GpsDeviceID").getValue() != "" && Ext.getCmp("GpsDeviceID").getValue() != null)
                                    {
                                        CS('CZCLZ.Handler.SaveYunDan', function (retVal) {
                                            if (retVal) {
                                                Ext.Msg.alert("提示", "制单成功！", function () {
                                                    FrameStack.popFrame();
                                                });
                                            } else {
                                                Ext.Msg.alert("提示", "制单失败！");
                                                return false;
                                            }
                                        }, CS.onError, Ext.getCmp("QiShiZhan_Province").getValue(), Ext.getCmp("QiShiZhan_City").getValue(), Ext.getCmp("DaoDaZhan_Province").getValue(), Ext.getCmp("DaoDaZhan_City").getValue(), Ext.getCmp("SuoShuGongSi").getValue(), Ext.getCmp("UserDenno").getValue(), Ext.getCmp("GpsDeviceID").getValue(), Ext.getCmp("YunDanRemark").getValue());
                                    }
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
    Ext.getCmp("HiddenID").setValue("");
    Ext.getCmp('HiddenID').focus(true, true);
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

Ext.define('PickCompany', {
    extend: 'Ext.window.Window',

    height: document.documentElement.clientHeight - 200,
    width: document.documentElement.clientWidth / 4,
    layout: {
        type: 'fit'
    },
    title: '公司选择',
    id:'pcWin',
    modal: true,
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    id: 'xy',
                    layout: {
                        type: 'fit'
                    },
                    items:[
                      {
                          xtype: 'gridpanel',
                          columnLines: 1,
                          border: 1,
                          store: gsStore,
                          autoScroll:true,
                          columns: [
                              {
                                  xtype: 'gridcolumn',
                                  dataIndex: 'SuoShuGongSi',
                                  flex: 3,
                                  sortable: false,
                                  menuDisabled: true,
                                  text: '公司'
                              },
                              {
                                  xtype: 'gridcolumn',
                                  flex: 1,
                                  sortable: false,
                                  menuDisabled: true,
                                  text: '操作',
                                  renderer: function (value,cellmeta,record,rowIndex,columnIndex,store) {
                                      return "<a href = 'javascript:void(0);' onClick='PickCom(\"" + record.data.SuoShuGongSi + "\");'>选择</a>";
                                  }
                              }
                          ]
                      }
                    ],
                    buttonAlign: 'center',
                    buttons: [
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

Ext.define('XYWin', {
    extend: 'Ext.window.Window',

    height: document.documentElement.clientHeight - 50,
    width: document.documentElement.clientWidth / 2,
    layout: {
        type: 'fit'
    },
    title: '协议说明',

    modal: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    id:'xy',
                    layout: {
                        type: 'fit'
                    },
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

function Show() {
    var win = new XYWin();
    win.show(null, function () {
        Ext.getCmp("xy").update("<iframe frameborder='0' src='http://chb.yk56.net/APPXieYi_Web.html' width='100%' height='100%'></iframe>");
    });
}

function PickCom(gs)
{
    Ext.getCmp("SuoShuGongSi").setValue(gs);
    Ext.getCmp("pcWin").close();
}