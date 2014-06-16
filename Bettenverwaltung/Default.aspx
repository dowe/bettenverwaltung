﻿<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Bettenverwaltung._Default" %>

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
                            <asp:Panel ID="divTabDetails" runat="server" CssClass="divTabInActive">
                                <div class="divTabContent">
                                    <div style="margin: 21px;">
                                        <div class="divFormLeft">
                                            <span class="spanLabel">PatientenId:</span>
                                            <asp:TextBox ID="txtBoxDetailsPatId" runat="server" ReadOnly="true" BackColor="LightGray"></asp:TextBox><br />

                                            <span class="spanLabel">Name:</span>
                                            <asp:TextBox ID="txtBoxDetailsPatName" runat="server" ReadOnly="true" BackColor="LightGray"></asp:TextBox><br />

                                            <span class="spanLabel">Geschlecht:</span>
                                            <asp:TextBox ID="txtBoxDetailsPatGender" runat="server" ReadOnly="true" BackColor="LightGray"></asp:TextBox><br />

                                            <span class="spanLabel">Geburtsdatum:</span>
                                            <asp:TextBox ID="txtBoxDetailsPatBirthday" runat="server" ReadOnly="true" BackColor="LightGray"></asp:TextBox><br />

                                            <span class="spanLabel">Korrekte Station:</span>
                                            <asp:TextBox ID="txtBoxDetailsPatCorrectStation" runat="server" ReadOnly="true" BackColor="LightGray"></asp:TextBox><br />

                                        </div>
                                        <div class="divFormRight">
                                            <span class="spanLabel">BettID:</span>
                                            <asp:TextBox ID="txtBoxDetailsBedId" runat="server" ReadOnly="true" BackColor="LightGray"></asp:TextBox><br />

                                            <span class="spanLabel">Station des Bettes:</span>
                                            <asp:TextBox ID="txtBoxDetailsBedStation" runat="server" ReadOnly="true" BackColor="LightGray"></asp:TextBox><br />


                                            <asp:ListBox ID="ListBox1" runat="server" CssClass="listDetailsPatHistory"></asp:ListBox>
                                            <br />
                                            <br />
                                            <asp:LinkButton runat="server" ID="btnDetailsDismiss" CssClass="btnFormSubmit">Dismiss</asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="divTabSearch" runat="server" CssClass="divTabActive">
                                <div class="divTabContent">
                                    <div style="margin:21px;">
                                        <span class="spanLabel">Patientensuche (ID oder Name):</span>
                                        <asp:TextBox ID="txtBoxSearchQuery" runat="server" CssClass="txtBoxSearchQuery"></asp:TextBox>
                                        <asp:LinkButton ID="btnSearch" runat="server" CssClass="btnSearch">Suche</asp:LinkButton>
                                        <asp:Panel ID="divSearchResultList" runat="server" CssClass="divSearchResultList">
                                            <asp:LinkButton ID="btnSearchResultListItem0" runat="server" CssClass="btnSearchResultListItem">
                                                <asp:Label ID="lblPatID0" runat="server" Text="1234" CssClass="lblPatId"></asp:Label> - Patrick Ewig, Station Orthopädie
                                            </asp:LinkButton>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="divTabAdd" runat="server" CssClass="divTabInactive">
                                <div class="divTabContent">
                                    <div style="margin: 21px;">
                                        <div class="divFormLeft">
                                            <asp:Label ID="lblAddPatFirstName" runat="server" Text="Vorname:" CssClass="spanLabel"></asp:Label>
                                            <asp:TextBox ID="txtBoxAddPatFirstName" runat="server"></asp:TextBox><br />

                                            <asp:Label ID="lblAddPatLastName" runat="server" Text="Nachname:" CssClass="spanLabel"></asp:Label>
                                            <asp:TextBox ID="txtBoxAddPatLastName" runat="server"></asp:TextBox><br />

                                            <asp:Label ID="lblAddPatGender" runat="server" Text="Geschlecht:" CssClass="spanLabel"></asp:Label>
                                            <asp:DropDownList ID="dropDownListAddPatGender" runat="server">
                                                <asp:ListItem>m</asp:ListItem>
                                                <asp:ListItem>w</asp:ListItem>
                                            </asp:DropDownList>

                                            <asp:Label ID="lblAddPatBirthday" runat="server" Text="Geburtsdatum:" CssClass="spanLabel"></asp:Label>
                                            <asp:TextBox ID="txtBoxAddPatBirthday" runat="server"></asp:TextBox><br />

                                            <asp:Label ID="lblAddPatStation" runat="server" Text="Station:" CssClass="spanLabel"></asp:Label>
                                            <asp:DropDownList ID="dropDownListAddPatStation" runat="server">
                                                <asp:ListItem>Pädiatrie</asp:ListItem>
                                                <asp:ListItem>Gynäkologie</asp:ListItem>
                                                <asp:ListItem>Innere Medizin</asp:ListItem>
                                                <asp:ListItem>Orthopädie</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="divFormRight" style="padding-top: 189px;">
                                            <asp:LinkButton runat="server" ID="btnAddConfirm" CssClass="btnFormSubmit" OnClick="Add_Patient_Click">Patient hinzufügen</asp:LinkButton>
                                        </div>
                                        <div style="clear: both;"></div>
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
