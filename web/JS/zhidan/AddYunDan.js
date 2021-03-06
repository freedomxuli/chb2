﻿var newcity = {};
var newqx = {};

var xuhao = 0;
var writechange = 1;//1:扫码；2：输入；

var province = Ext.create('Ext.data.Store', {
    fields: [
        'ID', 'MC'
    ]
});

var province2 = Ext.create('Ext.data.Store', {
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
                        columnWidth: 0.6,
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
                                xtype: 'fieldset',
                                title: '必填区',
                                columnWidth: 1,
                                layout: {
                                    type: 'column'
                                },
                                items: [
                                    {
                                        xtype: 'label',
                                        columnWidth: 1,
                                        html: "<div style='color:red;'>打开页面先扫描枪扫码，如果获取不到数据，请点击重置设备号再次尝试！</div>",
                                    },
                                    {
                                        xtype: 'textfield',
                                        columnWidth: 0.5,
                                        padding: '0 20 10 20',
                                        id: 'GpsDeviceID',
                                        readOnly: true,
                                        emptyText: '请扫码设备码',
                                        fieldLabel: '设备码'
                                    },
                                    {
                                        xtype: 'button',
                                        iconCls: 'close',
                                        text: '重置设备号',
                                        columnWidth: 0.25,
                                        margin: '0 10 0 0',
                                        id: 'CZSBH',
                                        handler: function () {
                                            Ext.getCmp("GpsDeviceID").setValue("");
                                            Ext.getCmp("HiddenID").setValue("");
                                            Ext.getCmp('HiddenID').focus(true, true);
                                        }
                                    },
                                    {
                                        xtype: 'button',
                                        iconCls: 'book',
                                        text: '切换至输入设备码',
                                        columnWidth: 0.25,
                                        margin: '0 10 0 0',
                                        handler: function () {
                                            if (writechange == 1) {//当等于1时说明要切换成输入码，所以要删除hiddenid
                                                Ext.getCmp("GpsDeviceID").setValue("");
                                                Ext.getCmp("HiddenID").setValue("");
                                                Ext.getCmp('HiddenID').focus(true, true);
                                                writechange = 2;//赋值输入
                                                Ext.getCmp("GpsDeviceID").setReadOnly(false);
                                                Ext.getCmp("CZSBH").hide();
                                                Ext.getCmp("GpsDeviceID").focus(true, true);
                                                Ext.getCmp("GpsDeviceID").emptyText = '请输入设备码';
                                                Ext.getCmp("GpsDeviceID").applyEmptyText();
                                                this.setText("切换至扫码设备码");
                                            } else {
                                                Ext.getCmp("GpsDeviceID").setValue("");
                                                writechange = 1;//赋值扫码
                                                Ext.getCmp("GpsDeviceID").setReadOnly(true);
                                                Ext.getCmp("CZSBH").show();
                                                Ext.getCmp("HiddenID").setValue("");
                                                Ext.getCmp('HiddenID').focus(true, true);
                                                Ext.getCmp("GpsDeviceID").emptyText = '请扫码设备码';
                                                Ext.getCmp("GpsDeviceID").applyEmptyText();
                                                this.setText("切换至输入设备码");
                                            }
                                        }
                                    },
                                    {
                                        xtype: 'container',
                                        columnWidth: 1,
                                        layout:'column',
                                        items: [
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
                                                anyMatch: true,
                                                typeAhead: true,
                                                forceSelection: true,
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
                                                typeAhead: true,
                                                forceSelection: true,
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
                                                id: 'QiShiZhan_Qx',
                                                typeAhead: true,
                                                forceSelection: true
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'combobox',
                                        columnWidth: 0.5,
                                        padding: '0 10 10 20',
                                        valueField: 'ID',
                                        displayField: 'MC',
                                        queryMode: 'local',
                                        store: province2,
                                        id: 'DaoDaZhan_Province',
                                        fieldLabel: '目的地',
                                        typeAhead: true,
                                        forceSelection: true,
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
                                        typeAhead: true,
                                        forceSelection: true,
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
                                        id: 'DaoDaZhan_Qx',
                                        typeAhead: true,
                                        forceSelection: true
                                    },
                                    {
                                        xtype: 'textfield',
                                        columnWidth: 0.5,
                                        padding: '0 10 10 20',
                                        id: 'SuoShuGongSi',
                                        allowBlank: false,
                                        fieldLabel: '建单公司'
                                    },
                                    {
                                        xtype: 'button',
                                        margin: '0 0 10 0',
                                        iconCls: 'add',
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
                                        columnWidth: 0.4,
                                        padding: '0 0 10 10',
                                        id: 'UserDenno',
                                        allowBlank: false,
                                        labelWidth:50,
                                        fieldLabel: '建单号'
                                    },
                                    {
                                        xtype: 'container',
                                        columnWidth: 1,
                                        layout:'column',
                                        items: [
                                            {
                                                xtype: 'numberfield',
                                                columnWidth: 0.6,
                                                padding: '0 10 10 20',
                                                id: 'Expect_Hour',
                                                allowDecimals: false,
                                                minValue: 1,
                                                fieldLabel: '预计到达时间（小时）'
                                            },
                                            {
                                                xtype: 'checkboxfield',
                                                columnWidth: 0.4,
                                                padding: '5 10 10 20',
                                                id: 'IsYJ',
                                                checked: true
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        columnWidth: 1,
                                        layout:'column',
                                        items: [
                                            {
                                                xtype: 'textfield',
                                                columnWidth: 0.45,
                                                padding: '0 10 10 20',
                                                id: 'MessageTel',
                                                emptyText: '请填入正确手机号',
                                                fieldLabel: '推送短信',
                                                regex: /^[1]\d{10}$/
                                            },
                                            {
                                                xtype: 'textfield',
                                                columnWidth: 0.25,
                                                padding: '0 10 10 0',
                                                emptyText: '请填入手机号，可为空',
                                                id: 'MessageTel1',
                                                regex: /^[1]\d{10}$/
                                            },
                                            {
                                                xtype: 'textfield',
                                                columnWidth: 0.25,
                                                padding: '0 10 10 0',
                                                emptyText: '请填入手机号，可为空',
                                                id: 'MessageTel2',
                                                regex: /^[1]\d{10}$/
                                            },
                                            {
                                                xtype: 'checkboxfield',
                                                columnWidth: 0.05,
                                                padding: '0 10 10 0',
                                                id: 'IsSendMessage',
                                                checked: true
                                            },
                                            {
                                                xtype: 'displayfield',
                                                columnWidth: 1,
                                                padding: '0 10 10 20',
                                                value: '<p style="color:red;">推送短信说明：仅在出发、预警状态短信提醒；最多可填写三个手机号码；可取消勾选，关闭短信提醒</p>'
                                            }
                                        ]
                                    }
                                    //{
                                    //    xtype: 'checkboxfield',
                                    //    columnWidth: 0.5,
                                    //    padding: '0 20 10 20',
                                    //    id: 'IsChuFaMessage',
                                    //    checked: true,
                                    //    fieldLabel: '出发提醒'
                                    //},
                                    //{
                                    //    xtype: 'checkboxfield',
                                    //    columnWidth: 0.5,
                                    //    padding: '0 20 10 10',
                                    //    id: 'IsDaoDaMessage',
                                    //    labelWidth: 60,
                                    //    checked: true,
                                    //    fieldLabel: '到达提醒'
                                    //}
                                ]
                            },
                            {
                                xtype: 'fieldset',
                                title: '选填区',
                                layout: {
                                    type: 'column'
                                },
                                columnWidth: 1,
                                items: [
                                    {
                                        xtype: 'textfield',
                                        columnWidth: 1,
                                        padding: '0 20 10 20',
                                        id: 'QiShiAddress',
                                        fieldLabel: '出发地详细地址'
                                    },
                                    {
                                        xtype: 'textfield',
                                        columnWidth: 1,
                                        padding: '0 20 10 20',
                                        id: 'DaoDaAddress',
                                        fieldLabel: '目的地详细地址'
                                    },
                                    {
                                        xtype: 'textfield',
                                        columnWidth: 0.5,
                                        padding: '0 0 10 20',
                                        id: 'SalePerson',
                                        fieldLabel: '销售员'
                                    },
                                    {
                                        xtype: 'textarea',
                                        columnWidth: 1,
                                        padding: '0 20 10 20',
                                        id: 'YunDanRemark',
                                        fieldLabel: '货物信息备注'
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
                                        xtype: 'displayfield',
                                        columnWidth: 1,
                                        padding: '0 20 10 20',
                                        fieldLabel: '特别说明',
                                        value: '<span style="color:red;">为保证客户隐私，目的地详细地址、收货人等信息只有在各平台“我的运单”中才能查看。</span>'
                                    }
                                ]
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
                                    //if (Ext.getCmp("GpsDeviceIDByHand").getValue() != "" && Ext.getCmp("GpsDeviceIDByHand").getValue() != null && Ext.getCmp("GpsDeviceID").getValue() != "" && Ext.getCmp("GpsDeviceID").getValue() != null)
                                    //{
                                    //    Ext.Msg.alert("提示", "扫描码和输入码不能同时存在！");
                                    //    return false;
                                    //}
                                    if ((Ext.getCmp("GpsDeviceID").getValue() == "" || Ext.getCmp("GpsDeviceID").getValue() == null)) {
                                        Ext.Msg.alert("提示", "扫描码或输入码必须填写一项！");
                                        return false;
                                    }

                                    if (Ext.getCmp("IsYJ").getValue())
                                    {
                                        if (Ext.getCmp("Expect_Hour").getValue() == "" || Ext.getCmp("Expect_Hour").getValue() == null) {
                                            Ext.Msg.alert("提示", "预计到达时间为必填项！");
                                            return false;
                                        }
                                    }

                                    var MessageTel = "";
                                    if (Ext.getCmp("IsSendMessage").getValue())
                                    {
                                        if (Ext.getCmp("MessageTel").getValue() == "" || Ext.getCmp("MessageTel").getValue() == null) {
                                            Ext.Msg.alert("提示", "推送短信为必填项！");
                                            return false;
                                        }
                                        if (Ext.getCmp("MessageTel").getValue() != "" && Ext.getCmp("MessageTel").getValue() != null)
                                        {
                                            MessageTel += Ext.getCmp("MessageTel").getValue()+",";
                                        }
                                        if (Ext.getCmp("MessageTel1").getValue() != "" && Ext.getCmp("MessageTel1").getValue() != null) {
                                            MessageTel += Ext.getCmp("MessageTel1").getValue() + ",";
                                        }
                                        if (Ext.getCmp("MessageTel2").getValue() != "" && Ext.getCmp("MessageTel2").getValue() != null) {
                                            MessageTel += Ext.getCmp("MessageTel2").getValue();
                                        }
                                    }
                                    
                                    if (writechange == 2)
                                    {
                                        CS('CZCLZ.Handler.IsBangBind', function (ret) {
                                            if (ret) {
                                                CS('CZCLZ.Handler.SaveYunDan', function (retVal) {
                                                    if (retVal) {
                                                        Ext.Msg.alert("提示", "制单成功！", function () {
                                                            location.reload();
                                                        });
                                                    } else {
                                                        Ext.Msg.alert("提示", "制单失败！");
                                                        return false;
                                                    }
                                                }, CS.onError, Ext.getCmp("QiShiZhan_Province").getValue(), Ext.getCmp("QiShiZhan_City").getValue(), Ext.getCmp("QiShiZhan_Qx").getValue(), Ext.getCmp("QiShiAddress").getValue(), Ext.getCmp("DaoDaZhan_Province").getValue(), Ext.getCmp("DaoDaZhan_City").getValue(), Ext.getCmp("DaoDaZhan_Qx").getValue(), Ext.getCmp("DaoDaAddress").getValue(), Ext.getCmp("SuoShuGongSi").getValue(), Ext.getCmp("UserDenno").getValue(), Ext.getCmp("Expect_Hour").getValue(), Ext.getCmp("SalePerson").getValue(), Ext.getCmp("Purchaser").getValue(), Ext.getCmp("PurchaserPerson").getValue(), Ext.getCmp("PurchaserTel").getValue(), Ext.getCmp("CarrierCompany").getValue(), Ext.getCmp("CarrierPerson").getValue(), Ext.getCmp("CarrierTel").getValue(), Ext.getCmp("GpsDeviceID").getValue(), Ext.getCmp("YunDanRemark").getValue(), Ext.getCmp("IsYJ").getValue(), Ext.getCmp("IsSendMessage").getValue(), MessageTel, details_array);
                                            } else {
                                                Ext.Msg.alert("提示", "输入码必须先解除绑定再制单！");
                                                return false;
                                            }
                                        }, CS.onError, Ext.getCmp("GpsDeviceID").getValue());
                                    } else if (Ext.getCmp("GpsDeviceID").getValue() != "" && Ext.getCmp("GpsDeviceID").getValue() != null)
                                    {
                                        CS('CZCLZ.Handler.SaveYunDan', function (retVal) {
                                            if (retVal) {
                                                Ext.Msg.alert("提示", "制单成功！", function () {
                                                    location.reload();
                                                });
                                            } else {
                                                Ext.Msg.alert("提示", "制单失败！");
                                                return false;
                                            }
                                        }, CS.onError, Ext.getCmp("QiShiZhan_Province").getValue(), Ext.getCmp("QiShiZhan_City").getValue(), Ext.getCmp("QiShiZhan_Qx").getValue(), Ext.getCmp("QiShiAddress").getValue(), Ext.getCmp("DaoDaZhan_Province").getValue(), Ext.getCmp("DaoDaZhan_City").getValue(), Ext.getCmp("DaoDaZhan_Qx").getValue(), Ext.getCmp("DaoDaAddress").getValue(), Ext.getCmp("SuoShuGongSi").getValue(), Ext.getCmp("UserDenno").getValue(), Ext.getCmp("Expect_Hour").getValue(), Ext.getCmp("SalePerson").getValue(), Ext.getCmp("Purchaser").getValue(), Ext.getCmp("PurchaserPerson").getValue(), Ext.getCmp("PurchaserTel").getValue(), Ext.getCmp("CarrierCompany").getValue(), Ext.getCmp("CarrierPerson").getValue(), Ext.getCmp("CarrierTel").getValue(), Ext.getCmp("GpsDeviceID").getValue(), Ext.getCmp("YunDanRemark").getValue(), Ext.getCmp("IsYJ").getValue(), Ext.getCmp("IsSendMessage").getValue(), MessageTel, details_array);
                                    }
                                }
                            },
                            //{
                            //    text: '返回',
                            //    iconCls: 'back',
                            //    handler: function () {
                            //        FrameStack.popFrame();
                            //    }
                            //},
                            {
                                text: '导出参考模板',
                                iconCls: 'download',
                                handler: function () {
                                    //FrameStack.popFrame();
                                    DownloadFile("CZCLZ.Handler.DownLoadMb", "制单模板.xlsx", "");
                                }
                            },
                            {
                                text: '导入模板',
                                iconCls: 'upload',
                                handler: function () {
                                    //FrameStack.popFrame();
                                    var win = new sjWin();
                                    win.show();
                                }
                            }
                        ]
                    },
                    {
                        xtype: 'gridpanel',
                        columnLines: 1,
                        border: 1,
                        height: document.documentElement.clientHeight,
                        columnWidth: 0.4,
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
    province2.loadData(provincesData);

    CS('CZCLZ.Handler.GetOldYunDan', function (retVal) {
        if (retVal)
        {
            if (retVal.length > 0)
            {
                var daodazhan = retVal[0]["QiShiZhan"].split(" ");
                Ext.getCmp("QiShiZhan_Province").setValue(daodazhan[0]);
                Ext.getCmp("QiShiZhan_City").setValue(daodazhan[1]);
                Ext.getCmp("SuoShuGongSi").setValue(retVal[0]["SuoShuGongSi"]);
                Ext.getCmp("QiShiZhan_Qx").setValue(retVal[0]["QiShiZhan_QX"]);
            }
        }
    },CS.onError)
}

Ext.define('sjWin', {
    extend: 'Ext.window.Window',

    height: 149,
    width: 350,
    layout: {
        type: 'fit'
    },
    closeAction: 'destroy',
    title: '数据导入',
    modal: true,

    initComponent: function () {
        var me = this;
        var dr_sbid = me.sbid;
        me.items = [
            {
                xtype: 'UploaderPanel',
                id: 'uploadproductpic',
                frame: true,
                bodyPadding: 10,
                title: '',
                items: [
                    {
                        xtype: 'filefield',
                        fieldLabel: '上传文件',
                        labelWidth: 70,
                        buttonText: '浏览',
                        allowBlank: false,
                        anchor: '100%'
                    }
                ]
                ,
                buttonAlign: 'center',
                buttons: [
                    {
                        text: '确定',
                        handler: function () {
                            var form = Ext.getCmp('uploadproductpic');
                            if (form.form.isValid()) {
                                //取得表单中的内容
                                var values = form.form.getValues(false);
                                var me = this;

                                //判断是否已经审核
                                Ext.getCmp('uploadproductpic').upload('CZCLZ.Handler.UploadSJ', function (retVal) {
                                    if (retVal) {
                                        if (retVal.dt[0]["GpsDeviceID"] != "" && retVal.dt[0]["GpsDeviceID"] != null)
                                            Ext.getCmp("GpsDeviceID").setValue(retVal.dt[0]["GpsDeviceID"]);
                                        else {
                                            Ext.Msg.alert("提示", "输入码不能为空！");
                                            return;
                                        }
                                        if (retVal.dt[0]["QiShiZhan_Province"] != "" && retVal.dt[0]["QiShiZhan_Province"] != null) {
                                            var provinceIndex = province.find("ID", retVal.dt[0]["QiShiZhan_Province"]);
                                            if (provinceIndex >= 0)
                                                Ext.getCmp("QiShiZhan_Province").setValue(retVal.dt[0]["QiShiZhan_Province"]);
                                            else {
                                                Ext.Msg.alert("提示", "出发地省份不规范，请重新填写或选择！");
                                                return;
                                            }
                                        } else {
                                            Ext.Msg.alert("提示", "出发地省份不能为空！");
                                            return;
                                        }
                                        if (retVal.dt[0]["QiShiZhan_City"] != "" && retVal.dt[0]["QiShiZhan_City"] != null) {
                                            var cityIndex = city.find("ID", retVal.dt[0]["QiShiZhan_City"]);
                                            if (cityIndex >= 0)
                                                Ext.getCmp("QiShiZhan_City").setValue(retVal.dt[0]["QiShiZhan_City"]);
                                            else {
                                                Ext.Msg.alert("提示", "出发地城市不规范，请重新填写或选择！");
                                                return;
                                            }
                                        } else {
                                            Ext.Msg.alert("提示", "出发地城市不能为空！");
                                            return;
                                        }
                                        if (retVal.dt[0]["QiShiZhan_Qx"] != "" && retVal.dt[0]["QiShiZhan_Qx"] != null) {
                                            var qxIndex = qx.find("ID", retVal.dt[0]["QiShiZhan_Qx"]);
                                            if (qxIndex >= 0)
                                                Ext.getCmp("QiShiZhan_Qx").setValue(retVal.dt[0]["QiShiZhan_Qx"]);
                                            else {
                                                Ext.Msg.alert("提示", "出发地区县不规范，请重新填写或选择！");
                                                return;
                                            }
                                        } else {
                                            Ext.Msg.alert("提示", "出发地区县不能为空！");
                                            return;
                                        }
                                        if (retVal.dt[0]["DaoDaZhan_Province"] != "" && retVal.dt[0]["DaoDaZhan_Province"] != null) {
                                            var provinceIndex2 = province2.find("ID", retVal.dt[0]["DaoDaZhan_Province"]);
                                            if (provinceIndex2 >= 0)
                                                Ext.getCmp("DaoDaZhan_Province").setValue(retVal.dt[0]["DaoDaZhan_Province"]);
                                            else {
                                                Ext.Msg.alert("提示", "目的地省份不规范，请重新填写或选择！");
                                                return;
                                            }
                                        } else {
                                            Ext.Msg.alert("提示", "目的地省份不能为空！");
                                            return;
                                        }
                                        if (retVal.dt[0]["DaoDaZhan_City"] != "" && retVal.dt[0]["DaoDaZhan_City"] != null) {
                                            var cityIndex2 = city2.find("ID", retVal.dt[0]["DaoDaZhan_City"]);
                                            if (cityIndex2 >= 0)
                                                Ext.getCmp("DaoDaZhan_City").setValue(retVal.dt[0]["DaoDaZhan_City"]);
                                            else {
                                                Ext.Msg.alert("提示", "目的地城市不规范，请重新填写或选择！");
                                                return;
                                            }
                                        } else {
                                            Ext.Msg.alert("提示", "目的地城市不能为空！");
                                            return;
                                        }
                                        if (retVal.dt[0]["DaoDaZhan_Qx"] != "" && retVal.dt[0]["DaoDaZhan_Qx"] != null) {
                                            var qxIndex2 = qx2.find("ID", retVal.dt[0]["DaoDaZhan_Qx"]);
                                            if (qxIndex2 >= 0)
                                                Ext.getCmp("DaoDaZhan_Qx").setValue(retVal.dt[0]["DaoDaZhan_Qx"]);
                                            else {
                                                Ext.Msg.alert("提示", "目的地区县不规范，请重新填写或选择！");
                                                return;
                                            }
                                        } else {
                                            Ext.Msg.alert("提示", "目的地区县不能为空！");
                                            return;
                                        }
                                        if (retVal.dt[0]["SuoShuGongSi"] != "" && retVal.dt[0]["SuoShuGongSi"] != null)
                                            Ext.getCmp("SuoShuGongSi").setValue(retVal.dt[0]["SuoShuGongSi"]);
                                        else {
                                            Ext.Msg.alert("提示", "建单公司不能为空！");
                                            return;
                                        }
                                        if (retVal.dt[0]["UserDenno"] != "" && retVal.dt[0]["UserDenno"] != null)
                                            Ext.getCmp("UserDenno").setValue(retVal.dt[0]["UserDenno"]);
                                        else {
                                            Ext.Msg.alert("提示", "建单号不能为空！");
                                            return;
                                        }
                                        if (retVal.dt[0]["Expect_Hour"] != "" && retVal.dt[0]["Expect_Hour"] != null)
                                            Ext.getCmp("Expect_Hour").setValue(retVal.dt[0]["Expect_Hour"]);
                                        else {
                                            Ext.Msg.alert("提示", "预计小时数不能为空！");
                                            return;
                                        }
                                        Ext.getCmp("MessageTel").setValue(retVal.dt[0]["MessageTel"]);
                                        Ext.getCmp("QiShiAddress").setValue(retVal.dt[0]["QiShiAddress"]);
                                        Ext.getCmp("QiShiAddress").setValue(retVal.dt[0]["QiShiAddress"]);
                                        Ext.getCmp("DaoDaAddress").setValue(retVal.dt[0]["DaoDaAddress"]);
                                        Ext.getCmp("SalePerson").setValue(retVal.dt[0]["SalePerson"]);
                                        Ext.getCmp("YunDanRemark").setValue(retVal.dt[0]["YunDanRemark"]);
                                        Ext.getCmp("CarrierCompany").setValue(retVal.dt[0]["CarrierCompany"]);
                                        Ext.getCmp("CarrierPerson").setValue(retVal.dt[0]["CarrierPerson"]);
                                        Ext.getCmp("CarrierTel").setValue(retVal.dt[0]["CarrierTel"]);
                                        Ext.getCmp("Purchaser").setValue(retVal.dt[0]["Purchaser"]);
                                        Ext.getCmp("PurchaserPerson").setValue(retVal.dt[0]["PurchaserPerson"]);
                                        Ext.getCmp("PurchaserTel").setValue(retVal.dt[0]["PurchaserTel"]);

                                        //if (retVal.dt_mx.length > 0)
                                        //{
                                        //    for (var i = 0; i < retVal.dt_mx.length; i++)
                                        //    {
                                        //        xuhao++;
                                        //        var add_record = [{
                                        //            'ID': xuhao,
                                        //            'GoodsName': retVal.dt_mx[i]["GoodsName"],
                                        //            'GoodsPack': retVal.dt_mx[i]["GoodsPack"],
                                        //            'GoodsNum': retVal.dt_mx[i]["GoodsNum"],
                                        //            'GoodsWeight': retVal.dt_mx[i]["GoodsWeight"],
                                        //            'GoodsVolume': retVal.dt_mx[i]["GoodsVolume"]
                                        //        }];

                                        //        detailStore.add(add_record);
                                        //    }
                                        //}
                                        me.up('window').close();
                                    }
                                }, function (err) {
                                    Ext.Msg.show({
                                        title: '错误',
                                        msg: err,
                                        buttons: Ext.MessageBox.OK,
                                        icon: Ext.MessageBox.ERROR,
                                        fn: function () {
                                            me.up('window').close();
                                        }
                                    });
                                }, 1);
                            }
                        }
                    },
                    {
                        text: '关闭',
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