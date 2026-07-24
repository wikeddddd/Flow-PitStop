<%@ Page Title="Advisor Dashboard" Language="C#" MasterPageFile="~/MemberSection.master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="PitStop.Admin.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MemberMainContent" runat="server">

    <h1 class="dashboard-greeting">Hi, <asp:Label ID="lblAdvisorName" runat="server" Text="Advisor"></asp:Label>! 👋</h1>
    <!-- TODO: [Teammate handling auth/data] bind lblAdvisorName to logged-in user -->

    <div class="stat-card">
        <div class="stat-header">
            <div class="stat-badge">👥</div>
            <span class="stat-level">Team Overview</span>
            <span class="stat-progress-tag">4 students &middot; 9 tasks assigned</span>
            <!-- TODO: bind team size + total task count dynamically -->
        </div>
        <div class="stat-bar-row">
            <span class="stat-bar-label">Done</span>
            <div class="stat-bar-track"><div class="stat-bar-fill xp" style="width: 62%;"></div></div>
            <span class="stat-bar-value">1/9 tasks</span>
        </div>
    </div>

    <div class="announce-card">
        <div class="card-heading">Pending Reviews</div>
        <div class="announce-item">
            <div class="announce-icon">📥</div>
            <div>
                <div class="announce-title">Sudhaarshan Nair - Section One Submission</div>
                <div class="announce-sub">Web Application Programming &middot; Awaiting review</div>
                <div class="announce-date">Submitted 18 July 2026</div>
            </div>
        </div>
        <div class="announce-item">
            <div class="announce-icon">📥</div>
            <div>
                <div class="announce-title">Danish Aiman - Aerodynamics Report</div>
                <div class="announce-sub">Design &amp; Aerodynamics &middot; Awaiting review</div>
                <div class="announce-date">Submitted 17 July 2026</div>
            </div>
        </div>
        <!-- TODO: bind pending submissions from database, link each to ReviewSubmissions.aspx -->
    </div>

    <div class="section-heading">Student Progress</div>
    <div class="course-grid">
        <div class="course-card">
            <div class="course-banner" style="background-color: #3d5a99;">SN</div>
            <div class="course-info">
                <div class="course-title">Sudhaarshan Nair</div>
                <div class="course-category">Level 4 &middot; 240/500 XP</div>
            </div>
        </div>
        <div class="course-card">
            <div class="course-banner" style="background-color: #6b3d99;">DA</div>
            <div class="course-info">
                <div class="course-title">Danish Aiman</div>
                <div class="course-category">Level 3 &middot; 180/500 XP</div>
            </div>
        </div>
        <div class="course-card">
            <div class="course-banner" style="background-color: #2d7a4f;">AA</div>
            <div class="course-info">
                <div class="course-title">Ahmad Nur Azami</div>
                <div class="course-category">Level 5 &middot; 310/500 XP</div>
            </div>
        </div>
        <!-- TODO: bind full student list dynamically, likely Repeater looping this card markup -->
    </div>

    <div class="section-heading">Recent Task Assignments</div>
    <div class="timeline-date">THIS WEEK</div>
    <div class="timeline-item">
        <div class="timeline-left">
            <span class="timeline-time">Due 20 Jul</span>
            <div>
                <div class="timeline-title">Aerodynamics Simulation Report</div>
                <div class="timeline-sub">Assigned to Danish Aiman</div>
            </div>
        </div>
        <div class="timeline-right">
            <span class="xp-tag">+30 XP</span>
            <button class="btn-submit">View Details</button>
        </div>
    </div>
    <!-- TODO: bind assigned tasks list, likely GridView/Repeater. AssignTask.aspx handles creating new ones -->

</asp:Content>