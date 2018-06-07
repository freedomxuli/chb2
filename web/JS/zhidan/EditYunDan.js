var UserID = window.queryString.UserID;
var YunDanDenno = window.queryString.YunDanDenno;

var newcity = {};
var newqx = {};

var xuhao = 0;
var writechange = 1;//1:扫码；2：输入；

var detailStore = Ext.create('Ext.data.Store', {
    fields: [
        'GoodsName', 'GoodsPack', 'GoodsNum', 'GoodsWeight', 'GoodsVolume', 'ID'
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
                        autoScroll: true,
                        layout: {
                            type: 'column'
                        },
                        border: 0,
                        items: [
                            {
                                xtype: 'fieldset',
                                title: '必填区',
                                columnWidth: 1,
                                layout: {
                                    type: 'column'
                                },
                                items: [
                                    {
                                        xtype: 'textfield',
                                        columnWidth: 0.5,
                                        padding: '0 20 10 20',
                                        id: 'GpsDeviceID',
                                        readOnly: true,
                                        fieldLabel: '设备码'
                                    },
                                    {
                                        xtype: 'container',
                                        columnWidth: 1,
                                        layout: 'column',
                                        items: [
                                            {
                                                xtype: 'textfield',
                                                columnWidth: 0.5,
                                                padding: '0 10 10 20',
                                                id: 'QiShiZhan_Province',
                                                readOnly: true,
                                                fieldLabel: '出发地'
                                            },
                                            {
                                                xtype: 'textfield',
                                                columnWidth: 0.25,
                                                padding: '0 10 10 0',
                                                id: 'QiShiZhan_City',
                                                readOnly: true
                                            },
                                            {
                                                xtype: 'textfield',
                                                columnWidth: 0.25,
                                                padding: '0 10 10 0',
                                                id: 'QiShiZhan_Qx',
                                                readOnly: true
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'textfield',
                                        columnWidth: 0.5,
                                        padding: '0 10 10 20',
                                        id: 'DaoDaZhan_Province',
                                        readOnly: true,
                                        fieldLabel: '目的地'
                                    },
                                    {
                                        xtype: 'textfield',
                                        columnWidth: 0.25,
                                        padding: '0 10 10 0',
                                        id: 'DaoDaZhan_City',
                                        readOnly: true
                                    },
                                    {
                                        xtype: 'textfield',
                                        columnWidth: 0.25,
                                        padding: '0 10 10 0',
                                        id: 'DaoDaZhan_Qx',
                                        readOnly: true
                                    },
                                    {
                                        xtype: 'textfield',
                                        columnWidth: 0.5,
                                        padding: '0 10 10 20',
                                        id: 'SuoShuGongSi',
                                        readOnly: true,
                                        fieldLabel: '建单公司'
                                    },
                                    {
                                        xtype: 'textfield',
                                        columnWidth: 0.5,
                                        padding: '0 10 10 10',
                                        id: 'UserDenno',
                                        readOnly: true,
                                        labelWidth: 50,
                                        fieldLabel: '建单号'
                                    },
                                    {
                                        xtype: 'textfield',
                                        columnWidth: 1,
                                        padding: '0 10 10 20',
                                        id: 'Expect_Hour',
                                        readOnly: true,
                                        minValue: 1,
                                        fieldLabel: '预计到达时间（小时）'
                                    },
                                    {
                                        xtype: 'container',
                                        columnWidth: 1,
                                        layout: 'column',
                                        items: [
                                            {
                                                xtype: 'textfield',
                                                columnWidth: 1,
                                                padding: '0 10 10 20',
                                                id: 'MessageTel',
                                                readOnly: true,
                                                fieldLabel: '推送短信'
                                            }
                                        ]
                                    }
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
                                        readOnly: true,
                                        fieldLabel: '出发地详细地址'
                                    },
                                    {
                                        xtype: 'textfield',
                                        columnWidth: 1,
                                        padding: '0 20 10 20',
                                        id: 'DaoDaAddress',
                                        readOnly: true,
                                        fieldLabel: '目的地详细地址'
                                    },
                                    {
                                        xtype: 'textfield',
                                        columnWidth: 0.5,
                                        padding: '0 0 10 20',
                                        id: 'SalePerson',
                                        readOnly: true,
                                        fieldLabel: '销售员'
                                    },
                                    {
                                        xtype: 'textarea',
                                        columnWidth: 1,
                                        padding: '0 20 10 20',
                                        id: 'YunDanRemark',
                                        readOnly: true,
                                        fieldLabel: '货物信息备注'
                                    },
                                    {
                                        xtype: 'textfield',
                                        columnWidth: 1,
                                        padding: '0 20 10 20',
                                        id: 'CarrierCompany',
                                        readOnly: true,
                                        fieldLabel: '承运公司（专线）'
                                    },
                                    {
                                        xtype: 'textfield',
                                        columnWidth: 0.5,
                                        padding: '0 0 10 20',
                                        id: 'CarrierPerson',
                                        readOnly: true,
                                        fieldLabel: '负责人'
                                    },
                                    {
                                        xtype: 'textfield',
                                        columnWidth: 0.5,
                                        padding: '0 20 10 10',
                                        id: 'CarrierTel',
                                        labelWidth: 60,
                                        readOnly: true,
                                        fieldLabel: '联系方式'
                                    },
                                    {
                                        xtype: 'textfield',
                                        columnWidth: 1,
                                        padding: '0 20 10 20',
                                        id: 'Purchaser',
                                        readOnly: true,
                                        fieldLabel: '收货单位'
                                    },
                                    {
                                        xtype: 'textfield',
                                        columnWidth: 0.5,
                                        padding: '0 0 10 20',
                                        id: 'PurchaserPerson',
                                        readOnly: true,
                                        fieldLabel: '收货人'
                                    },
                                    {
                                        xtype: 'textfield',
                                        columnWidth: 0.5,
                                        padding: '0 20 10 10',
                                        id: 'PurchaserTel',
                                        labelWidth: 60,
                                        readOnly: true,
                                        fieldLabel: '联系方式'
                                    }
                                ]
                            }
                        ],
                        buttonAlign: 'center',
                        buttons: [
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
    CS('CZCLZ.Handler.EditYunDan', function (retVal) {
        if (retVal)
        {
            Ext.getCmp("GpsDeviceID").setValue(retVal.dt[0]["GpsDeviceID"]);
            Ext.getCmp("QiShiZhan_Province").setValue(retVal.dt[0]["QiShiZhan"].split(' ')[0]);
            Ext.getCmp("QiShiZhan_City").setValue(retVal.dt[0]["QiShiZhan"].split(' ')[1]);
            Ext.getCmp("QiShiZhan_Qx").setValue(retVal.dt[0]["QiShiZhan_QX"]);
            Ext.getCmp("DaoDaZhan_Province").setValue(retVal.dt[0]["DaoDaZhan"].split(' ')[0]);
            Ext.getCmp("DaoDaZhan_City").setValue(retVal.dt[0]["DaoDaZhan"].split(' ')[1]);
            Ext.getCmp("DaoDaZhan_Qx").setValue(retVal.dt[0]["DaoDaZhan_QX"]);
            Ext.getCmp("SuoShuGongSi").setValue(retVal.dt[0]["SuoShuGongSi"]);
            Ext.getCmp("UserDenno").setValue(retVal.dt[0]["UserDenno"]);
            Ext.getCmp("Expect_Hour").setValue(retVal.dt[0]["Expect_Hour"]);
            Ext.getCmp("MessageTel").setValue(retVal.dt[0]["MessageTel"]);
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
            detailStore.loadData(retVal.dt_detail);
        }
    }, CS.onError, YunDanDenno);
}