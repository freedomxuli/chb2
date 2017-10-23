var msg = queryString.msg;

//***********************************主界面******************************************
Ext.onReady(function() {
    Ext.define('tabmenuView', {
        extend: 'Ext.container.Viewport',
        layout: {
            type: 'fit'
        },
        
        initComponent: function() {
            var me = this;

            Ext.applyIf(me, {
                items: [
                    {
                        xtype: 'tabpanel',
                        id:'tabpan',
                        activeTab: 0,
                        items: [
                            
                        ]
                    }
                ]
            });

            me.callParent(arguments);
        }
    });
    
    new tabmenuView();
    
    var tabmenu=Ext.getCmp("tabpan");
    var tdbmsg=msg.split("|");
    
    for(var i=0;i<tdbmsg.length;i++)
    {
        var panmsg=tdbmsg[i].split(",");
        var title=panmsg[0];
        var src=panmsg[1].replace('@','?');
        var pan =Ext.create('Ext.panel.Panel', {
            title: title,
            html: '<iframe name="mainframe" id="mainframe" frameborder="0" src="'+src+'" width="100%" height="100%">'
        });
        tabmenu.add(pan);
    }
    
    
});
//***********************************主界面******************************************
