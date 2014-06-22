<%@ Page Title="Bettenverwaltung" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Bettenverwaltung._Default" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <form id="form1" runat="server">
        <div class="divCenter">
            <h1>Bettenverwaltung 0.8</h1>
            <div class="divContent">
                <div class="divLeft">
                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                    <asp:Timer ID="timerUpdate" runat="server" Interval="30000" OnTick="Update_Overview_And_Notifications_Tick"></asp:Timer>
                    <asp:UpdatePanel ID="updatePanelOverview" ChildrenAsTriggers="true" runat="server" class="divOverview">
                        <ContentTemplate>
                            <asp:Panel ID="divStationPaediatrie" runat="server" CssClass="divOverviewStation marginRight">
                                <h2>Pädiatrie</h2>
                                <asp:Panel ID="divStationPaediatrieBeds" runat="server"></asp:Panel>
                            </asp:Panel>
                            <asp:Panel ID="divStationGynaekologie" runat="server" CssClass="divOverviewStation marginRight">
                                <h2>Gynäkologie</h2>
                                <asp:Panel ID="divStationGynaekologieBeds" runat="server"></asp:Panel>
                            </asp:Panel>
                            <asp:Panel ID="divStationInnereMedizin" runat="server" CssClass="divOverviewStation marginRight">
                                <h2>Innere Medizin</h2>
                                <asp:Panel ID="divStationInnereMedizinBeds" runat="server"></asp:Panel>
                            </asp:Panel>
                            <asp:Panel ID="divStationOrthopaedie" runat="server" CssClass="divOverviewStation">
                                <h2>Orthopädie</h2>
                                <asp:Panel ID="divStationOrthopaedieBeds" runat="server"></asp:Panel>
                            </asp:Panel>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="timerUpdate" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="updatePanelTabs" ChildrenAsTriggers="true" runat="server" class="divTabsContainer">
                        <ContentTemplate>
                            <asp:LinkButton ID="btnTabDetails" runat="server" OnClick="Tab_Details_Click" CssClass="btnTabActive">Details</asp:LinkButton><asp:LinkButton ID="btnTabSearch" runat="server" OnClick="Tab_Search_Click" CssClass="btnTabInactive">Suche</asp:LinkButton><asp:LinkButton ID="btnTabAdd" runat="server" OnClick="Tab_Add_Click" CssClass="btnTabInactive">Neuer Patient</asp:LinkButton>
                            <div class="divTabs">
                                <asp:Panel ID="divTabDetails" runat="server" CssClass="divTabInActive">
                                    <div class="divTabContent">
                                        <div style="margin: 21px;">
                                            <div class="divFormLeft">
                                                <span class="spanLabel">PatientenId:</span>
                                                <asp:TextBox ID="txtBoxDetailsPatId" runat="server" ReadOnly="true" BackColor="LightGray" onfocus="pauseTimer();" onblur="restartTimer();"></asp:TextBox><br />

                                                <span class="spanLabel">Name:</span>
                                                <asp:TextBox ID="txtBoxDetailsPatName" runat="server" ReadOnly="true" BackColor="LightGray" onfocus="pauseTimer();" onblur="restartTimer();"></asp:TextBox><br />

                                                <span class="spanLabel">Geschlecht:</span>
                                                <asp:TextBox ID="txtBoxDetailsPatGender" runat="server" ReadOnly="true" BackColor="LightGray" onfocus="pauseTimer();" onblur="restartTimer();"></asp:TextBox><br />

                                                <span class="spanLabel">Geburtsdatum:</span>
                                                <asp:TextBox ID="txtBoxDetailsPatBirthday" runat="server" ReadOnly="true" BackColor="LightGray" onfocus="pauseTimer();" onblur="restartTimer();"></asp:TextBox><br />

                                                <span class="spanLabel">Korrekte Station:</span>
                                                <asp:TextBox ID="txtBoxDetailsPatCorrectStation" runat="server" ReadOnly="true" BackColor="LightGray" onfocus="pauseTimer();" onblur="restartTimer();"></asp:TextBox><br />

                                            </div>
                                            <div class="divFormRight">
                                                <span class="spanLabel">BettID:</span>
                                                <asp:TextBox ID="txtBoxDetailsBedId" runat="server" ReadOnly="true" BackColor="LightGray" onfocus="pauseTimer();" onblur="restartTimer();"></asp:TextBox><br />

                                                <span class="spanLabel">Station des Bettes:</span>
                                                <asp:TextBox ID="txtBoxDetailsBedStation" runat="server" ReadOnly="true" BackColor="LightGray" onfocus="pauseTimer();" onblur="restartTimer();"></asp:TextBox><br />


                                                <asp:TextBox ID="txtBoxDetailsPatHistory" runat="server" CssClass="txtBoxDetailsPatHistory" TextMode="MultiLine" Wrap="False"></asp:TextBox>
                                                <br />
                                                <br />
                                                <asp:LinkButton runat="server" ID="btnDetailsDismiss" CssClass="btnFormSubmit" OnClick="Dismiss_Patient_Click">Patient entlassen</asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="divTabSearch" runat="server" CssClass="divTabActive">
                                    <div class="divTabContent">
                                        <div style="margin: 21px;">
                                            <span class="spanLabel">Patientensuche (ID oder Name):</span>
                                            <asp:TextBox ID="txtBoxSearchQuery" runat="server" CssClass="txtBoxSearchQuery" onfocus="pauseTimer();" onblur="restartTimer();"></asp:TextBox>
                                            <asp:LinkButton ID="btnSearch" runat="server" CssClass="btnSearch" OnClick="Search_Click">Suche</asp:LinkButton>
                                            <asp:Panel ID="divSearchResultList" runat="server" CssClass="divSearchResultList"></asp:Panel>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="divTabAdd" runat="server" CssClass="divTabInactive">
                                    <div class="divTabContent">
                                        <div style="margin: 21px;">
                                            <div class="divFormLeft">
                                                <asp:Label ID="lblAddPatFirstName" runat="server" Text="Vorname:" CssClass="spanLabel"></asp:Label>
                                                <asp:TextBox ID="txtBoxAddPatFirstName" runat="server" onfocus="pauseTimer();" onblur="restartTimer();"></asp:TextBox><br />

                                                <asp:Label ID="lblAddPatLastName" runat="server" Text="Nachname:" CssClass="spanLabel"></asp:Label>
                                                <asp:TextBox ID="txtBoxAddPatLastName" runat="server" onfocus="pauseTimer();" onblur="restartTimer();"></asp:TextBox><br />

                                                <asp:Label ID="lblAddPatGender" runat="server" Text="Geschlecht:" CssClass="spanLabel"></asp:Label>
                                                <asp:DropDownList ID="dropDownListAddPatGender" runat="server" onfocus="pauseTimer();" onblur="restartTimer();">
                                                    <asp:ListItem>m</asp:ListItem>
                                                    <asp:ListItem>w</asp:ListItem>
                                                </asp:DropDownList>

                                                <asp:Label ID="lblAddPatBirthday" runat="server" Text="Geburtsdatum:" CssClass="spanLabel"></asp:Label>
                                                <asp:TextBox ID="txtBoxAddPatBirthday" runat="server" onfocus="pauseTimer();" onblur="restartTimer();"></asp:TextBox><br />

                                                <asp:Label ID="lblAddPatStation" runat="server" Text="Station:" CssClass="spanLabel"></asp:Label>
                                                <asp:DropDownList ID="dropDownListAddPatStation" runat="server" onfocus="pauseTimer();" onblur="restartTimer();">
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
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnAddConfirm" />
                            <asp:PostBackTrigger ControlID="btnDetailsDismiss" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>

                <div class="divRight">
                    <asp:UpdatePanel ID="updatePanelNot" ChildrenAsTriggers="true" runat="server" class="divNotList">
                        <ContentTemplate>
                            <asp:Panel ID="divNotifications" runat="server"></asp:Panel>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="timerUpdate" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <script>
                        $("divNotList").ready(openClosedNotifications);
                    </script>
                </div>
            </div>
        </div>
        <asp:Panel ID="divOverlay" runat="server" CssClass="divOverlay">
            <asp:Panel ID="divMessageBox" runat="server" CssClass="divMessageBox">
                <div style="margin: 10px;">
                    <asp:Label ID="lblMessageBoxText" Text="TestNachricht 123123123123 sadf sadfjvb hababababable.?" runat="server" CssClass="spanMessageBoxText"></asp:Label>
                    <div style="margin: 10px">
                        <asp:Button ID="btnMessageBoxOkay" Text="OK" runat="server" CssClass="btnMessageBoxOkay" />
                        <asp:Button ID="btnMessageBoxCancel" Text="Abbrechen" Visible="false" runat="server" CssClass="btnMessageBoxCancel" />
                    </div>
                </div>
            </asp:Panel>
        </asp:Panel>
    </form>
</asp:Content>
