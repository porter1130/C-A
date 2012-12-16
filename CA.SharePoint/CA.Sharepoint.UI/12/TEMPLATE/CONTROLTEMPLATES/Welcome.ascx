<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Control Language="C#" Inherits="Microsoft.SharePoint.WebControls.Welcome,Microsoft.SharePoint,Version=12.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" AutoEventWireup="false" CompilationMode="Always" %>
<SharePoint:PersonalActions AccessKey="<%$Resources:wss,personalactions_menu_ak%>" ToolTip="<%$Resources:wss,open_menu%>" runat="server" ID="ExplicitLogout" Visible="false">
    <CustomTemplate>
        <SharePoint:FeatureMenuTemplate runat="server" FeatureScope="Site" Location="Microsoft.SharePoint.StandardMenu" GroupId="PersonalActions" ID="ID_PersonalActionMenu" UseShortId="true">
            <%-- <SharePoint:MenuItemTemplate runat="server" id="ID_PersonalInformation"
				 Text="<%$Resources:wss,personalactions_personalinformation%>"
				 Description="<%$Resources:wss,personalactions_personalinformationdescription%>"
				 MenuGroupId="100"
				 Sequence="100"
				 ImageUrl="/_layouts/images/menuprofile.gif"
				 UseShortId="true"
				 />--%>
            <SharePoint:MenuItemTemplate runat="server" ID="ID_LoginAsDifferentUser" Text="<%$Resources:wss,personalactions_loginasdifferentuser%>" Description="<%$Resources:wss,personalactions_loginasdifferentuserdescription%>" MenuGroupId="200" Sequence="100" UseShortId="true" />
            <SharePoint:MenuItemTemplate runat="server" ID="ID_RequestAccess" Text="<%$Resources:wss,personalactions_requestaccess%>" Description="<%$Resources:wss,personalactions_requestaccessdescription%>" MenuGroupId="200" UseShortId="true" Sequence="200" />
            <SharePoint:MenuItemTemplate runat="server" ID="ID_Logout" Text="<%$Resources:wss,personalactions_logout%>" Description="<%$Resources:wss,personalactions_logoutdescription%>" MenuGroupId="200" Sequence="300" UseShortId="true" />
            <%-- <SharePoint:MenuItemTemplate runat="server" id="ID_PersonalizePage"
				 Text="<%$Resources:wss,personalactions_personalizepage%>"
				 Description="<%$Resources:wss,personalactions_personalizepagedescription%>"
				 ImageUrl="/_layouts/images/menupersonalize.gif"
				 ClientOnClickScript="javascript:MSOLayout_ChangeLayoutMode(true);"
				 PermissionsString="AddDelPrivateWebParts,UpdatePersonalWebParts"
				 PermissionMode="Any"
				 MenuGroupId="300"
				 Sequence="100"
				 UseShortId="true"
				 />
		 <SharePoint:MenuItemTemplate runat="server" id="ID_SwitchView"
				 MenuGroupId="300"
				 Sequence="200"
				 UseShortId="true"
				 />
		 <SharePoint:MenuItemTemplate runat="server" id="MSOMenu_RestoreDefaults"
				 Text="<%$Resources:wss,personalactions_restorepagedefaults%>"
				 Description="<%$Resources:wss,personalactions_restorepagedefaultsdescription%>"
				 ClientOnClickNavigateUrl="javascript:MSOWebPartPage_RestorePageDefault()"
				 MenuGroupId="300"
				 Sequence="300"
				 UseShortId="true"
				 />--%>
           <SharePoint:MenuItemTemplate runat="server" ID="ID_CreateDelegation" Text="<%$Resources:ca,create_delegation%>" Description="<%$Resources:ca,create_delegation_description%>" ClientOnClickNavigateUrl="/WorkFlowCenter/Lists/Delegates/NewForm.aspx" MenuGroupId="300" Sequence="400" UseShortId="true" />
            <%--<SharePoint:MenuItemTemplate runat="server" ID="ID_ViewDelegation" Text="<%$Resources:ca,view_delegation%>" Description="<%$Resources:ca,view_delegation_description%>" ClientOnClickNavigateUrl="/WorkFlowCenter/Lists/Delegates/AllItems.aspx" MenuGroupId="300" Sequence="500" UseShortId="true" />--%>
        </SharePoint:FeatureMenuTemplate>
    </CustomTemplate>
</SharePoint:PersonalActions>
<SharePoint:ApplicationPageLink runat="server" ID="ExplicitLogin" ApplicationPageFileName="Authenticate.aspx" AppendCurrentPageUrl="true" Text="<%$Resources:wss,login_pagetitle%>" Style="display: none" Visible="false" />