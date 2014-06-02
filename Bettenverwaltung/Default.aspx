<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Bettenverwaltung._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <form id="form1" runat="server">
        <div class="divContent">
            <h1>Bettenverwaltung 0.01</h1>
            <div class="divLeft">
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <asp:UpdatePanel ID="divOverview" runat="server" class="divOverview">
                    <ContentTemplate>
                        <asp:Panel ID="divStationPaediatrie" runat="server" CssClass="divOverviewStation">
                            <h2>Pädiatrie</h2>
                        </asp:Panel>
                        <asp:Panel ID="divStationGynaekologie" runat="server" CssClass="divOverviewStation">
                            <h2>Gynäkologie</h2>
                        </asp:Panel>
                        <asp:Panel ID="divStationInnereMedizin" runat="server" CssClass="divOverviewStation">
                            <h2>Innere Medizin</h2>
                        </asp:Panel>
                        <asp:Panel ID="divStationOrthopaedie" runat="server" CssClass="divOverviewStation">
                            <h2>Orthopädie</h2>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="divTabs">
                    <ul></ul>
                    <asp:Panel ID="divTabDetails" runat="server" CssClass="divTab">
                        asdkfasdfl
                    </asp:Panel>
                </div>
            </div>

            <div class="divRight">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" class="divNotList">
                    <ContentTemplate>
                        <asp:Panel ID="not0" runat="server" CssClass="divNotListItem">
                            Mustermann, Max (PID 4711)<br />
                            Derzeit:    Bett 123,   Station Innere Medizin<br />
                            Nach:       Bett 157,   Station Orthopädie
                            <asp:LinkButton ID="rel0" runat="server" OnClick="Accept_Relocation_Click" CssClass="btnAcceptRel">
                                Annehmen
                            </asp:LinkButton>
                        </asp:Panel>
                        <asp:Panel ID="not1" runat="server" CssClass="divNotListItem">
                            Mustermann2, Max (PID 4711)<br />
                            Derzeit:    Bett 123,   Station Innere Medizin<br />
                            Nach:       Bett 157,   Station Orthopädie
                            <asp:LinkButton ID="rel1" runat="server" OnClick="Accept_Relocation_Click" CssClass="btnAcceptRel">
                                Annehmen
                            </asp:LinkButton>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <script>
                    $("divNotList").ready(openClosedNotifications);
                </script>
            </div>
        </div>
    </form>
</asp:Content>
