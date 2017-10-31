var pageSize = 15;

var czStore = createSFW4Store({
    pageSize: pageSize,
    total: 1,
    currentPage: 1,
    fields: [
       'ChongZhiTime', 'ChongZhiJinE', 'ChongZhiRemark'
    ],
    onPageChange: function (sto, nPage, sorters) {
        DataBind(nPage);
    }
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
                        store: czStore,
                        columns: [
                            {
                                xtype: 'datecolumn',
                                dataIndex: 'ChongZhiTime',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                format: 'Y-m-d',
                                text: '日期'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'ChongZhiJinE',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '金额'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'ChongZhiRemark',
                                flex: 2,
                                sortable: false,
                                menuDisabled: true,
                                text: '充值描述'
                            }
                        ],
                        dockedItems: [
                            {
                                xtype: 'toolbar',
                                dock: 'top',
                                items: [
                                    {
                                        xtype: 'datefield',
                                        editable: false,
                                        labelWidth: 60,
                                        width: 200,
                                        id: 'startTime',
                                        format:'Y-m-d',
                                        fieldLabel: '日期'
                                    },
                                    {
                                        xtype: 'label',
                                        text: '至'
                                    },
                                    {
                                        xtype: 'datefield',
                                        editable: false,
                                        id: 'endTime',
                                        format: 'Y-m-d',
                                        width: 140
                                    },
                                    {
                                        xtype: 'button',
                                        iconCls: 'search',
                                        text: '查询',
                                        handler: function () {
                                            if (Ext.getCmp("startTime").getValue() != "" && Ext.getCmp("startTime").getValue() != null && Ext.getCmp("endTime").getValue() != "" && Ext.getCmp("endTime").getValue() != null) {
                                                DataBind(1);
                                            } else {
                                                Ext.Msg.alert("提示", "日期必填！");
                                                return false;
                                            }
                                        }
                                    }
                                ]
                            },
                            {
                                xtype: 'pagingtoolbar',
                                dock: 'bottom',
                                width: 360,
                                store: czStore,
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

    DataBind(1);
});

function DataBind(cp) {
    CS('CZCLZ.Handler.XFJL', function (retVal) {
        if (retVal)
        {
            czStore.setData({
                data: retVal.dt,
                pageSize: pageSize,
                total: retVal.ac,
                currentPage: retVal.cp
            });
        }
    }, CS.onError, cp, pageSize, Ext.getCmp("startTime").getValue(), Ext.getCmp("endTime").getValue());
}
