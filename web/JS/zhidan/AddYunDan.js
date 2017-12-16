var newcity = {};
var newqx = {};

var xuhao = 0;

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

var gsStore = Ext.create('Ext.data.Store', {
    fields: [
        'SuoShuGongSi'
    ]
});

var detailStore = Ext.create('Ext.data.Store', {
    fields: [
        'GoodsName', 'GoodsPack', 'GoodsNum', 'GoodsWeight', 'GoodsVolume','ID'
    ]
});

Ext.onReady(function () {
    Ext.define('MainView', {
        extend: 'Ext.container.Viewport',

        layout: {
            //align: 'center',
            type: 'column'
        },

        initComponent: function () {
            var me = this;

            Ext.applyIf(me, {
                items: [
                    {
                        xtype: 'panel',
                        height: document.documentElement.clientHeight,
                        columnWidth: 0.5,
                        autoScroll:true,
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
                                columnWidth: 0.8,
                                padding: '0 20 10 20',
                                id: 'GpsDeviceID',
                                readOnly: true,
                                fieldLabel: '扫描码'
                            },
                            {
                                xtype: 'button',
                                iconCls: 'close',
                                text: '重置设备号',
                                columnWidth: 0.2,
                                margin: '0 10 0 0',
                                handler: function () {
                                    Ext.getCmp("GpsDeviceID").setValue("");
                                    Ext.getCmp("HiddenID").setValue("");
                                    Ext.getCmp('HiddenID').focus(true, true);
                                }
                            },
                            {
                                xtype: 'textfield',
                                columnWidth: 0.8,
                                padding: '0 20 10 20',
                                id: 'GpsDeviceIDByHand',
                                fieldLabel: '输入码'
                            },
                            {
                                xtype: 'combobox',
                                columnWidth: 0.5,
                                padding: '0 10 10 20',
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
                                columnWidth: 0.25,
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
                                columnWidth: 0.25,
                                padding: '0 10 10 0',
                                valueField: 'ID',
                                displayField: 'MC',
                                queryMode: 'local',
                                store: qx,
                                id: 'QiShiZhan_Qx'
                            },
                            {
                                xtype: 'textfield',
                                columnWidth: 1,
                                padding: '0 20 10 20',
                                id: 'QiShiAddress',
                                allowBlank: false,
                                fieldLabel: '出发地详细地址'
                            },
                            {
                                xtype: 'combobox',
                                columnWidth: 0.5,
                                padding: '0 10 10 20',
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
                                columnWidth: 0.25,
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
                                columnWidth: 0.25,
                                padding: '0 10 10 0',
                                valueField: 'ID',
                                displayField: 'MC',
                                queryMode: 'local',
                                store: qx2,
                                id: 'DaoDaZhan_Qx'
                            },
                            {
                                xtype: 'textfield',
                                columnWidth: 1,
                                padding: '0 20 10 20',
                                id: 'DaoDaAddress',
                                allowBlank: false,
                                fieldLabel: '目的地详细地址'
                            },
                            {
                                xtype: 'textfield',
                                columnWidth: 0.8,
                                padding: '0 20 10 20',
                                id: 'SuoShuGongSi',
                                allowBlank: false,
                                fieldLabel: '建单公司'
                            },
                            {
                                xtype: 'button',
                                margin: '0 0 10 10',
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
                                columnWidth: 0.6,
                                padding: '0 0 10 20',
                                id: 'UserDenno',
                                allowBlank: false,
                                fieldLabel: '建单号'
                            },
                            {
                                xtype: 'textfield',
                                columnWidth: 0.4,
                                padding: '0 20 10 10',
                                id: 'SalePerson',
                                labelWidth:50,
                                fieldLabel: '销售员'
                            },
                            {
                                xtype: 'textfield',
                                columnWidth: 1,
                                padding: '0 20 10 20',
                                id: 'Purchaser',
                                fieldLabel: '收货单位'
                            },
                            {
                                xtype: 'textfield',
                                columnWidth: 0.5,
                                padding: '0 0 10 20',
                                id: 'PurchaserPerson',
                                fieldLabel: '收货人'
                            },
                            {
                                xtype: 'textfield',
                                columnWidth: 0.5,
                                padding: '0 20 10 10',
                                id: 'PurchaserTel',
                                labelWidth: 60,
                                fieldLabel: '联系方式'
                            },
                            {
                                xtype: 'textfield',
                                columnWidth: 1,
                                padding: '0 20 10 20',
                                id: 'CarrierCompany',
                                fieldLabel: '承运公司（专线）'
                            },
                            {
                                xtype: 'textfield',
                                columnWidth: 0.5,
                                padding: '0 0 10 20',
                                id: 'CarrierPerson',
                                fieldLabel: '负责人'
                            },
                            {
                                xtype: 'textfield',
                                columnWidth: 0.5,
                                padding: '0 20 10 10',
                                id: 'CarrierTel',
                                labelWidth: 60,
                                fieldLabel: '联系方式'
                            },
                            {
                                xtype: 'checkboxfield',
                                columnWidth: 0.5,
                                padding: '0 20 10 20',
                                id: 'IsChuFaMessage',
                                checked:true,
                                fieldLabel: '出发提醒'
                            },
                            {
                                xtype: 'checkboxfield',
                                columnWidth: 0.5,
                                padding: '0 20 10 10',
                                id: 'IsDaoDaMessage',
                                labelWidth: 60,
                                checked: true,
                                fieldLabel: '到达提醒'
                            },
                            {
                                xtype: 'textarea',
                                columnWidth: 1,
                                padding: '0 20 10 20',
                                id: 'YunDanRemark',
                                fieldLabel: '备注'
                            },
                            {
                                xtype: 'label',
                                columnWidth: 1,
                                padding: '0 20 10 20',
                                html: '<a href="javascript:void(0);" onclick="Show();" style="float:left;">请先阅读相关协议</a>'
                            }
                        ],
                        buttonAlign: 'center',
                        buttons: [
                            {
                                text: '制单',
                                iconCls: 'enable',
                                handler: function () {
                                    var details_array = [];
                                    for(var i = 0;i<detailStore.data.items.length;i++)
                                    {
                                        var obj_detail = {};
                                        obj_detail["GoodsName"] = detailStore.data.items[i].get("GoodsName");
                                        obj_detail["GoodsPack"] = detailStore.data.items[i].get("GoodsPack");
                                        obj_detail["GoodsNum"] = detailStore.data.items[i].get("GoodsNum");
                                        obj_detail["GoodsWeight"] = detailStore.data.items[i].get("GoodsWeight");
                                        obj_detail["GoodsVolume"] = detailStore.data.items[i].get("GoodsVolume");
                                        details_array.push(obj_detail);
                                    }
                                    if (Ext.getCmp("QiShiZhan_Province").getValue() == "" || Ext.getCmp("QiShiZhan_Province").getValue() == null)
                                    {
                                        Ext.Msg.alert("提示", "出发地省份为必填项！");
                                        return false;
                                    }
                                    if (Ext.getCmp("QiShiZhan_City").getValue() == "" || Ext.getCmp("QiShiZhan_City").getValue() == null) {
                                        Ext.Msg.alert("提示", "出发地城市为必填项！");
                                        return false;
                                    }
                                    if (Ext.getCmp("QiShiZhan_Qx").getValue() == "" || Ext.getCmp("QiShiZhan_Qx").getValue() == null) {
                                        Ext.Msg.alert("提示", "出发地区县为必填项！");
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
                                    if (Ext.getCmp("DaoDaZhan_Qx").getValue() == "" || Ext.getCmp("DaoDaZhan_Qx").getValue() == null) {
                                        Ext.Msg.alert("提示", "目的地区县为必填项！");
                                        return false;
                                    }
                                    if (Ext.getCmp("SuoShuGongSi").getValue() == "" || Ext.getCmp("SuoShuGongSi").getValue() == null) {
                                        Ext.Msg.alert("提示", "建单公司为必填项！");
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
                                                }, CS.onError, Ext.getCmp("QiShiZhan_Province").getValue(), Ext.getCmp("QiShiZhan_City").getValue(), Ext.getCmp("QiShiZhan_Qx").getValue(), Ext.getCmp("QiShiAddress").getValue(), Ext.getCmp("DaoDaZhan_Province").getValue(), Ext.getCmp("DaoDaZhan_City").getValue(), Ext.getCmp("DaoDaZhan_Qx").getValue(), Ext.getCmp("DaoDaAddress").getValue(), Ext.getCmp("SuoShuGongSi").getValue(), Ext.getCmp("UserDenno").getValue(), Ext.getCmp("SalePerson").getValue(), Ext.getCmp("Purchaser").getValue(), Ext.getCmp("PurchaserPerson").getValue(), Ext.getCmp("PurchaserTel").getValue(), Ext.getCmp("CarrierCompany").getValue(), Ext.getCmp("CarrierPerson").getValue(), Ext.getCmp("CarrierTel").getValue(), Ext.getCmp("IsChuFaMessage").getValue(), Ext.getCmp("IsDaoDaMessage").getValue(), Ext.getCmp("GpsDeviceIDByHand").getValue(), Ext.getCmp("YunDanRemark").getValue(), details_array);
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
                                        }, CS.onError, Ext.getCmp("QiShiZhan_Province").getValue(), Ext.getCmp("QiShiZhan_City").getValue(), Ext.getCmp("QiShiZhan_Qx").getValue(), Ext.getCmp("QiShiAddress").getValue(), Ext.getCmp("DaoDaZhan_Province").getValue(), Ext.getCmp("DaoDaZhan_City").getValue(), Ext.getCmp("DaoDaZhan_Qx").getValue(), Ext.getCmp("DaoDaAddress").getValue(), Ext.getCmp("SuoShuGongSi").getValue(), Ext.getCmp("UserDenno").getValue(), Ext.getCmp("SalePerson").getValue(), Ext.getCmp("Purchaser").getValue(), Ext.getCmp("PurchaserPerson").getValue(), Ext.getCmp("PurchaserTel").getValue(), Ext.getCmp("CarrierCompany").getValue(), Ext.getCmp("CarrierPerson").getValue(), Ext.getCmp("CarrierTel").getValue(), Ext.getCmp("IsChuFaMessage").getValue(), Ext.getCmp("IsDaoDaMessage").getValue(), Ext.getCmp("GpsDeviceIDByHand").getValue(), Ext.getCmp("YunDanRemark").getValue(), details_array);
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
                    },
                    {
                        xtype: 'gridpanel',
                        columnLines: 1,
                        border: 1,
                        height: document.documentElement.clientHeight,
                        columnWidth: 0.5,
                        autoScroll: true,
                        store: detailStore,
                        id: 'detailStore',
                        columns: [
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'GoodsName',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '货物名称'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'GoodsPack',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '包装'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'GoodsNum',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '数量'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'GoodsWeight',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '重量'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'GoodsVolume',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '体积'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'ID',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '操作',
                                renderer: function (value,cellmeta,record,rowIndex,columnIndex,store) {
                                    return "<a href='javascript:void(0);' onClick='DelDetail(\"" + value + "\");'>删除</a>";
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
                                        iconCls: 'add',
                                        text: '新增详情',
                                        handler: function () {
                                            var win = new AddDetailWin();
                                            win.show(null, function () {

                                            });
                                        }
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

    cityBind();

});

function DelDetail(id)
{
    Ext.getCmp("detailStore").store.remove(Ext.getCmp("detailStore").store.findRecord("ID", id));
}

function cityBind() {
    Ext.getCmp("HiddenID").setValue("");
    Ext.getCmp('HiddenID').focus(true, true);
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
            for (var k = 0; k < cityData3[i].children[j].children.length; k++)
            {
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
                                  text: '建单公司'
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

Ext.define('AddDetailWin', {
    extend: 'Ext.window.Window',

    height: document.documentElement.clientHeight - 50,
    width: document.documentElement.clientWidth / 2,
    layout: {
        type: 'fit'
    },
    title: '新增明细',

    modal: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    id: 'DetailWin',
                    layout: {
                        type: 'column'
                    },
                    items:[
                        {
                            xtype: 'textfield',
                            columnWidth: 1,
                            padding: 20,
                            id: 'GoodsName',
                            fieldLabel: '货物名称'
                        },
                        {
                            xtype: 'textfield',
                            columnWidth: 1,
                            padding: 20,
                            id: 'GoodsPack',
                            fieldLabel: '包装'
                        },
                        {
                            xtype: 'textfield',
                            columnWidth: 1,
                            padding: 20,
                            id: 'GoodsNum',
                            fieldLabel: '数量'
                        },
                        {
                            xtype: 'textfield',
                            columnWidth: 1,
                            padding: 20,
                            id: 'GoodsWeight',
                            fieldLabel: '重量'
                        },
                        {
                            xtype: 'textfield',
                            columnWidth: 1,
                            padding: 20,
                            id: 'GoodsVolume',
                            fieldLabel: '体积'
                        }
                    ],
                    buttonAlign: 'center',
                    buttons: [
                        {
                            text: '确认',
                            iconCls: 'add',
                            handler: function () {

                                xuhao++;

                                var add_record = [{
                                    'ID': xuhao,
                                    'GoodsName': Ext.getCmp("GoodsName").getValue(),
                                    'GoodsPack': Ext.getCmp("GoodsPack").getValue(),
                                    'GoodsNum': Ext.getCmp("GoodsNum").getValue(),
                                    'GoodsWeight': Ext.getCmp("GoodsWeight").getValue(),
                                    'GoodsVolume': Ext.getCmp("GoodsVolume").getValue()
                                }];

                                detailStore.add(add_record);

                                me.close();
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