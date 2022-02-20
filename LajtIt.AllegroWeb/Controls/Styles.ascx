<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Styles.ascx.cs" Inherits="LajtIt.AllegroWeb.Controls.Styles" %>
<style>
    #container
    {
        background-image: url('http://www.lajtit.pl/public/assets/allegro/lajtit_background.png');
        width: 100%;
        text-align: center;
        font-family: Arial;
        font-size:16px;
        background-color: Silver;
    }
    #page
    {
        margin: auto;
        width: 980px;
        border: solid 1px #00B0EB;
        background-color: White;
        text-align: left;
    }
    #header
    {
        position: relative;
        width: 980px;
        height: 174px;
        background-image: url('http://www.lajtit.pl/public/assets/allegro/ljatit_blog_banner.jpg');
    }
    #header_flags
    {
        width: 330px;
        height: 50px;
        position: absolute;
        right: 310px;
        top: 0px; 
        text-align:right;
        padding: 2px 2px 2px 0; 
    }
    #header_links
    {
        width: 300px;
        height: 20px;
        position: absolute;
        right: 0px;
        top: 0px;
        background-color: Gray;
        padding: 2px 2px 2px 0;
        text-align: right;
        font-weight:bold;
        color:White;
    }
    
    #header_links a
    {
        color: White;
        font-size: 12px;
    }
    #short
    {
        width: 200px;
        height: 100px;
        background-color: White;
        border: solid 3px #E60A80;
        right: 10px;
        top: 32px;
        position: absolute;
        text-align: left;
        padding: 15px;
        font-size: 12px;
    }
    #arrange
    {
        padding: 0 30px 30px 30px;
        font-size: 10px;
    }
    #arrange table
    {
        width: 100%;
    }
    #arrange table td
    {
        width: 50%;
        text-align: center;
    }
    #arrange table td img
    {
        width: 400px;
    }
    #offer
    {
        padding: 0 30px 30px 30px;
    }
    .offer_main_photo
    {
        float: left;
        width: 600px;
        text-align:center;
    }
    .offer_main_photo_frame
    {
        border: solid 1px #00B0EB;
        padding: 10px;
        margin: 0 10px 10px 0;
    } 
    .border
    {        
        border: solid 1px #00B0EB;
    }
    .offer_main_photo_frame img
    {
        max-width:560px;
    } 
    .offer_spec
    {
        float: left;
        width: 260px;
        padding: 10px;
    }
    .offer_spec_comments
    {
    clear: left; text-align: right; font-size: 12px; padding:30px;
    }
    .offer_spec table
    {
        font-size: 12px;
        font-family: Tahoma;
    }
    .offer_spec table td
    {
        background-color: #ebebeb;
        padding: 4px;
    }
    #contact
    {
        width: 970px;
        height: 25px;
        background-color: Gray;
        padding: 5px;
        text-align: center;
    }
    #contact a
    {
        color: White;
        text-decoration: underline;
    }
    .color-it
    {
        color: #B0CC1F;
    }
    .color-lajt
    {
        color: #00B0EB;
    }
    .color-r
    {
        color: #E60A80;
    }
    .color-white
    {
        color: #ffffff;
    }
    .bold
    {
        font-weight: bold;
    }
    .regular_text
    {
        padding: 15px 10px 15px 10px;
        text-indent: 10px;
    }
    .item_title
    {
        margin:10px 0 10px 0;
        font-size: 26px;
    }
    #suggestions table.t1  {padding:0px;margin:0px; border:0px; width:980px ;height:300px; }
    #suggestions table.t1 td  {padding:0px;margin:0px; border:0px;  }
    .table_row_header { text-decoration:bold; font-size:18pt;}
    .table_row_desc {font-size:8pt; text-align:center;}
    .table_row_img {text-align:center;}      
    
    div.price
    {text-align: center; position: absolute; right: 10px; top: 0px; width: 200px;
        height: 100px; background-image: url('http://www.lajtit.pl/public/assets/allegro/cena_zawiera.jpg');
    }     
    div.price div   {font-size: 18pt; margin: 35px;}
    div.offer_main_photo_comment {font-size: 10pt;}
    div.hidden {font-size:1px; color:White;}
    
    td.green{background-color:Green; font-size:10pt; color:White;}
    tr.black{background-color:#00B0EB;  color:White; font-weight:bold;}
    
    
    .special_box {font-size: 14pt; color: #E60A80; border: 10px solid #00B0EB;background-image: url('http://www.lajtit.pl/public/assets/allegro/lajtit_background.png') ; background-position:right; background-repeat:no-repeat; padding: 40px 70px 40px 40px; padding-right:180px;}
</style>
