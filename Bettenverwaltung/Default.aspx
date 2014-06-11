<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Bettenverwaltung._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <form id="form1" runat="server">
        <div class="divCenter">
            <h1>Bettenverwaltung 0.01</h1>
            <div class="divContent">
                <div class="divLeft">
                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                    <asp:UpdatePanel ID="divOverview" runat="server" class="divOverview">
                        <ContentTemplate>
                            <asp:Panel ID="divStationPaediatrie" runat="server" CssClass="divOverviewStation marginRight">
                                <h2>Pädiatrie</h2>
                            </asp:Panel>
                            <asp:Panel ID="divStationGynaekologie" runat="server" CssClass="divOverviewStation marginRight">
                                <h2>Gynäkologie</h2>
                            </asp:Panel>
                            <asp:Panel ID="divStationInnereMedizin" runat="server" CssClass="divOverviewStation marginRight">
                                <h2>Innere Medizin</h2>
                            </asp:Panel>
                            <asp:Panel ID="divStationOrthopaedie" runat="server" CssClass="divOverviewStation">
                                <h2>Orthopädie</h2>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="divTabsContainer">
                        <asp:LinkButton ID="btnTabDetails" runat="server" OnClick="Tab_Details_Click" CssClass="btnTabActive">Details</asp:LinkButton><asp:LinkButton ID="btnTabSearch" runat="server" OnClick="Tab_Search_Click" CssClass="btnTabInactive">Suche</asp:LinkButton><asp:LinkButton ID="btnTabAdd" runat="server" OnClick="Tab_Add_Click" CssClass="btnTabInactive">Neuer Patient</asp:LinkButton>
                        <div class="divTabs">
                            <asp:Panel ID="divTabDetails" runat="server" CssClass="divTabInactive">
                                <div class="divTabContent">
                                    <div class="divFormLeft">
                                        <div style="float:left;">
                                            PatientenId:<br /><br />
                                            Name:<br /><br />
                                            Geschlecht:<br /><br />
                                            Geburtsdatum:<br /><br />
                                            Korrekte Station:<br /><br />
                                        </div>
                                        <div style="float:right; right: 0; margin-right:30px;">
                                            <asp:TextBox ID="txtBoxDetailsPatId" runat="server" ReadOnly="true" BackColor="LightGray"></asp:TextBox><br /><br />
                                            <asp:TextBox ID="txtBoxDetailsPatName" runat="server" ReadOnly="true" BackColor="LightGray"></asp:TextBox><br /><br />
                                            <asp:TextBox ID="txtBoxDetailsPatGender" runat="server" ReadOnly="true" BackColor="LightGray"></asp:TextBox><br /><br />
                                            <asp:TextBox ID="txtBoxDetailsPatBirthday" runat="server" ReadOnly="true" BackColor="LightGray"></asp:TextBox><br /><br />
                                            <asp:TextBox ID="txtBoxDetailsPatCorrectStation" runat="server" ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="divFormRight">
                                        <div style="float:left;">
                                            BettId:<br /><br />
                                            Station des Bettes:<br /><br />
                                        </div>
                                        <div style="float:right; right: 0;">
                                            <asp:TextBox ID="txtBoxDetailsBedId" runat="server" ReadOnly="true" BackColor="LightGray"></asp:TextBox><br /><br />
                                            <asp:TextBox ID="txtBoxDetailsBedStation" runat="server" ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                        </div>
                                        <asp:ListBox ID="ListBox1" runat="server" CssClass="listDetailsPatHistory"></asp:ListBox>
                                        <br /><br />
                                        <asp:LinkButton runat="server" ID="btnDetailsDismiss" CssClass="btnFormSubmit">Dismiss</asp:LinkButton>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="divTabSearch" runat="server" CssClass="divTabActive">
                                <div class="divTabContent">
                                    <div style="margin: 21px;">
                                       
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="divTabAdd" runat="server" CssClass="divTabInactive">
                                <div class="divTabContent">
                                    <div class="divFormLeft">
                                        <div style="float:left;">
                                            <asp:Label ID="lblAddPatFirstName" runat="server" Text="Vorname:"></asp:Label><br /><br />
                                            <asp:Label ID="lblAddPatLastName" runat="server" Text="Name:"></asp:Label><br /><br />
                                             <asp:Label ID="lblAddPatGender" runat="server" Text="Geschlecht:"></asp:Label><br /><br />
                                             <asp:Label ID="lblAddPatBirthday" runat="server" Text="Geburtsdatum:"></asp:Label><br /><br />
                                             <asp:Label ID="lblAddPatStation" runat="server" Text="Station:"></asp:Label><br /><br />
                                        </div>
                                        <div style="float:right; right: 0; margin-right:30px;">
                                            <asp:TextBox ID="txtBoxAddPatFirstName" runat="server" ></asp:TextBox><br /><br />
                                            <asp:TextBox ID="txtBoxAddPatLastName" runat="server"></asp:TextBox><br /><br />
                                            <asp:TextBox ID="txtBoxAddPatGender" runat="server"></asp:TextBox><br /><br />
                                            <asp:TextBox ID="txtBoxAddPatBirthday" runat="server"></asp:TextBox><br /><br />
                                            <asp:TextBox ID="txtBoxAddPatCorrectStation" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="divFormRight">
                                        <br /><br /><br /><br /><br /><br /><br /><br />
                                        <asp:LinkButton runat="server" ID="btnAddConfirm" CssClass="btnFormSubmit">Patient hinzufügen</asp:LinkButton>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>

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
        </div>
    </form>
</asp:Content>
