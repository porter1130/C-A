
//DateTimePicker 1.0 jianyi0115@163.com
//�ͻ��˽ű������������ߵĴ���,�����ԣ�http://dev.csdn.net/develop/article/65/65724.shtm
//----------------------------------------------------------------------
//
// ����: ����Nick.Lee��
//
// ASP.net�µ�ǰ̨�����ؼ�Դ����(��ˢ��ҳ��)

// ʱ��:2005-3-15

// boyorgril@msn.com
// QQ:16503096
//ע�⣺�������������,лл
//----------------------------------------------------------------------
//
//
//==================================================== �����趨���� =======================================================
var bMoveable=true;  //���������Ƿ�����϶�
var _VersionInfo=""         //�汾��Ϣ

//==================================================== WEB ҳ����ʾ���� =====================================================
var strFrame;  //����������HTML����
document.writeln('<iframe id=DateTimePicker_DateLayer scrolling=0 frameborder=0 style="position: absolute; width: 144; height: 211; z-index: 9998; display: none"></iframe>');
strFrame='<style>';
strFrame+='INPUT.button{BORDER-RIGHT: #9496E1 1px solid;BORDER-TOP: #9496E1 1px solid;BORDER-LEFT: #9496E1 1px solid;';
strFrame+='BORDER-BOTTOM: #9496E1 1px solid;BACKGROUND-COLOR: #fff8ec;font-family:����;}';
strFrame+='TD{FONT-SIZE: 9pt;font-family:����;}';
strFrame+='</style>';
strFrame+='<scr' + 'ipt>';
strFrame+='var datelayerx,datelayery; /*��������ؼ������λ��*/';
strFrame+='var bDrag; /*����Ƿ�ʼ�϶�*/';
strFrame+='function document.onmousemove() /*������ƶ��¼��У������ʼ�϶����������ƶ�����*/';
strFrame+='{if(bDrag && window.event.button==1)';
strFrame+=' {var DateLayer=parent.document.all.DateTimePicker_DateLayer.style;';
strFrame+='  DateLayer.posLeft += window.event.clientX-datelayerx;/*����ÿ���ƶ��Ժ����λ�ö��ָ�Ϊ��ʼ��λ�ã����д����div�в�ͬ*/';
strFrame+='  DateLayer.posTop += window.event.clientY-datelayery;}}';
strFrame+='function DragStart()  /*��ʼ�����϶�*/';
strFrame+='{var DateLayer=parent.document.all.DateTimePicker_DateLayer.style;';
strFrame+=' datelayerx=window.event.clientX;';
strFrame+=' datelayery=window.event.clientY;';
strFrame+=' bDrag=true;}';
strFrame+='function DragEnd(){  /*���������϶�*/';
strFrame+=' bDrag=false;}';
strFrame+='</scr' + 'ipt>';
strFrame+='<div style="z-index:9999;position: absolute; left:0; top:0;" onselectstart="return false"><span id=tmpSelectYearLayer  style="z-index: 9999;position: absolute;top: 3; left: 19;display: none"></span>';
strFrame+='<span id=tmpSelectMonthLayer  style="z-index: 9999;position: absolute;top: 3; left: 78;display: none"></span>';
strFrame+='<table border=1 cellspacing=0 cellpadding=0 width=142 height=160 bordercolor=#9496E1 bgcolor=#9496E1 >';
strFrame+='  <tr ><td width=142 height=23  bgcolor=#FFFFFF><table border=0 cellspacing=1 cellpadding=0 width=140  height=23>';
strFrame+='      <tr align=center ><td width=16 align=center bgcolor=#9496E1 style="font-size:12px;cursor: hand;color: #FFFFFF" ';
strFrame+='        onclick="parent.nickPrevM()" title="��ǰ�� 1 ��" ><b >&lt;</b>';
strFrame+='        </td><td width=60 align=center style="font-size:12px;cursor:default"  ';
strFrame+='onmouseover="style.backgroundColor=\'#C6C7EF\'" onmouseout="style.backgroundColor=\'white\'" ';
strFrame+='onclick="parent.tmpSelectYearInnerHTML(this.innerText.substring(0,4))" title="�������ѡ�����"><span  id=nickYearHead></span></td>';
strFrame+='<td width=48 align=center style="font-size:12px;cursor:default"  onmouseover="style.backgroundColor=\'#C6C7EF\'" ';
strFrame+=' onmouseout="style.backgroundColor=\'white\'" onclick="parent.tmpSelectMonthInnerHTML(this.innerText.length==3?this.innerText.substring(0,1):this.innerText.substring(0,2))"';
strFrame+='        title="�������ѡ���·�"><span id=nickMonthHead ></span></td>';
strFrame+='        <td width=16 bgcolor=#9496E1 align=center style="font-size:12px;cursor: hand;color: #FFFFFF" ';
strFrame+='         onclick="parent.nickNextM()" title="��� 1 ��" ><b >&gt;</b></td></tr>';
strFrame+='    </table></td></tr>';
strFrame+='  <tr ><td width=142 height=18 >';
strFrame+='<table border=1 cellspacing=0 cellpadding=0 bgcolor=#9496E1 ' + (bMoveable? 'onmousedown="DragStart()" onmouseup="DragEnd()"':'');
strFrame+=' BORDERCOLORLIGHT=#9496E1 BORDERCOLORDARK=#FFFFFF width=140 height=20  style="cursor:' + (bMoveable ? 'move':'default') + '">';
strFrame+='<tr  align=center valign=bottom><td style="font-size:12px;color:#FFFFFF" >��</td>';
strFrame+='<td style="font-size:12px;color:#FFFFFF" >һ</td><td style="font-size:12px;color:#FFFFFF" >��</td>';
strFrame+='<td style="font-size:12px;color:#FFFFFF" >��</td><td style="font-size:12px;color:#FFFFFF" >��</td>';
strFrame+='<td style="font-size:12px;color:#FFFFFF" >��</td><td style="font-size:12px;color:#FFFFFF" >��</td></tr>';
strFrame+='</table></td></tr>';
strFrame+='  <tr ><td width=142 height=120 >';
strFrame+='    <table border=1 cellspacing=2 cellpadding=0 BORDERCOLORLIGHT=#9496E1 BORDERCOLORDARK=#FFFFFF bgcolor=#fff8ec width=140 height=120 >';
var n=0; for (j=0;j<5;j++){ strFrame+= ' <tr align=center >'; for (i=0;i<7;i++){
strFrame+='<td width=20 height=20 id=nickDay'+n+' style="font-size:12px"  onclick=parent.nickDayClick(this.innerText,0)></td>';n++;}
strFrame+='</tr>';}
strFrame+='      <tr align=center >';
for (i=35;i<39;i++)strFrame+='<td width=20 height=20 id=nickDay'+i+' style="font-size:12px"  onclick="parent.nickDayClick(this.innerText,0)"></td>';
strFrame+='        <td colspan=3 align=right ><span onclick=parent.closeLayer() style="font-size:12px;cursor: hand"';
strFrame+='          title="' + _VersionInfo + '"><u>�ر�</u></span>&nbsp;</td></tr>';
strFrame+='    </table></td></tr><tr ><td >';
strFrame+='        <table border=0 cellspacing=1 cellpadding=0 width=100%  bgcolor=#FFFFFF>';
strFrame+='          <tr ><td  align=left><input  type=button class=button value="<<" title="��ǰ�� 1 ��" onclick="parent.nickPrevY()" ';
strFrame+='             onfocus="this.blur()" style="font-size: 12px; height: 20px"><input  class=button title="��ǰ�� 1 ��" type=button ';
strFrame+='             value="< " onclick="parent.nickPrevM()" onfocus="this.blur()" style="font-size: 12px; height: 20px"></td><td ';
strFrame+='              align=center><input  type=button class=button value=���� onclick="parent.nickToday()" ';
strFrame+='             onfocus="this.blur()" title="��ǰ����" style="font-size: 12px; height: 20px; cursor:hand"></td><td ';
strFrame+='              align=right><input  type=button class=button value=" >" onclick="parent.nickNextM()" ';
strFrame+='             onfocus="this.blur()" title="��� 1 ��" class=button style="font-size: 12px; height: 20px"><input ';
strFrame+='              type=button class=button value=">>" title="��� 1 ��" onclick="parent.nickNextY()"';
strFrame+='             onfocus="this.blur()" style="font-size: 12px; height: 20px"></td>';
strFrame+='</tr></table></td></tr></table></div>';

window.frames.DateTimePicker_DateLayer.document.writeln(strFrame);
window.frames.DateTimePicker_DateLayer.document.close();  //���ie������������������

//==================================================== WEB ҳ����ʾ���� ======================================================
var outObject;
var outButton;  //����İ�ť
var outDate="";  //��Ŷ��������
var odatelayer=window.frames.DateTimePicker_DateLayer.document.all;  //�����������
function DateTimePicker_setday(tt,obj) //��������
{
 if (arguments.length >  2){alert("�Բ��𣡴��뱾�ؼ��Ĳ���̫�࣡");return;}
 if (arguments.length == 0){alert("�Բ�����û�д��ر��ؼ��κβ�����");return;}
 var dads  = document.all.DateTimePicker_DateLayer.style;
 var th = tt;
 var ttop  = tt.offsetTop;     //TT�ؼ��Ķ�λ���
 var thei  = tt.clientHeight;  //TT�ؼ�����ĸ�
 var tleft = tt.offsetLeft;    //TT�ؼ��Ķ�λ���
 var ttyp  = tt.type;          //TT�ؼ�������
 while (tt = tt.offsetParent){ttop+=tt.offsetTop; tleft+=tt.offsetLeft;}
 dads.top  = (ttyp=="image")? ttop+thei : ttop+thei+6;
 dads.left = tleft;
 outObject = (arguments.length == 1) ? th : obj;
 outButton = (arguments.length == 1) ? null : th; //�趨�ⲿ����İ�ť
 //���ݵ�ǰ������������ʾ����������
 var reg = /^(\d+)-(\d{1,2})-(\d{1,2})$/;
 var r = outObject.value.match(reg);
 if(r!=null){
  r[2]=r[2]-1;
  var d= new Date(r[1], r[2],r[3]);
  if(d.getFullYear()==r[1] && d.getMonth()==r[2] && d.getDate()==r[3]){
   outDate=d;  //�����ⲿ���������
  }
  else outDate="";
   nickSetDay(r[1],r[2]+1);
 }
 else{
  outDate="";
  nickSetDay(new Date().getFullYear(), new Date().getMonth() + 1);
 }
 dads.display = '';

 event.returnValue=false;
}

var MonHead = new Array(12);         //����������ÿ���µ��������
    MonHead[0] = 31; MonHead[1] = 28; MonHead[2] = 31; MonHead[3] = 30; MonHead[4]  = 31; MonHead[5]  = 30;
    MonHead[6] = 31; MonHead[7] = 31; MonHead[8] = 30; MonHead[9] = 31; MonHead[10] = 30; MonHead[11] = 31;

var nickTheYear=new Date().getFullYear(); //������ı����ĳ�ʼֵ
var nickTheMonth=new Date().getMonth()+1; //�����µı����ĳ�ʼֵ
var nickWDay=new Array(39);               //����д���ڵ�����

function document.onclick() //������ʱ�رոÿؼ� //ie6�����������������л����㴦�����
{
  with(window.event)
  { if (srcElement.getAttribute("Author")==null && srcElement != outObject && srcElement != outButton)
    closeLayer();
  }
}

function document.onkeyup()  //��Esc���رգ��л�����ر�
  {
    if (window.event.keyCode==27){
  if(outObject)outObject.blur();
  closeLayer();
 }
 else if(document.activeElement)
  if(document.activeElement.getAttribute("Author")==null && document.activeElement != outObject && document.activeElement != outButton)
  {
   closeLayer();
  }
  }

function nickWriteHead(yy,mm)  //�� head ��д�뵱ǰ��������
  {
 odatelayer.nickYearHead.innerText  = yy + " ��";
    odatelayer.nickMonthHead.innerText = mm + " ��";
  }

function tmpSelectYearInnerHTML(strYear) //��ݵ�������
{
  if (strYear.match(/\D/)!=null){alert("�����������������֣�");return;}
  var m = (strYear) ? strYear : new Date().getFullYear();
  if (m < 1000 || m > 9999) {alert("���ֵ���� 1000 �� 9999 ֮�䣡");return;}
  var n = m - 10;
  if (n < 1000) n = 1000;
  if (n + 26 > 9999) n = 9974;
  var s = "<select  name=tmpSelectYear style='font-size: 12px' "
     s += "onblur='document.all.tmpSelectYearLayer.style.display=\"none\"' "
     s += "onchange='document.all.tmpSelectYearLayer.style.display=\"none\";"
     s += "parent.nickTheYear = this.value; parent.nickSetDay(parent.nickTheYear,parent.nickTheMonth)'>\r\n";
  var selectInnerHTML = s;
  n = n-70;
  for (var i = n; i < n + 200; i++)
  {
    if (i == m)
       {selectInnerHTML += "<option  value='" + i + "' selected>" + i + "��" + "</option>\r\n";}
    else {selectInnerHTML += "<option  value='" + i + "'>" + i + "��" + "</option>\r\n";}
  }
  selectInnerHTML += "</select>";
  odatelayer.tmpSelectYearLayer.style.display="";
  odatelayer.tmpSelectYearLayer.innerHTML = selectInnerHTML;
  odatelayer.tmpSelectYear.focus();
}

function tmpSelectMonthInnerHTML(strMonth) //�·ݵ�������
{
  if (strMonth.match(/\D/)!=null){alert("�·���������������֣�");return;}
  var m = (strMonth) ? strMonth : new Date().getMonth() + 1;
  var s = "<select  name=tmpSelectMonth style='font-size: 12px' "
     s += "onblur='document.all.tmpSelectMonthLayer.style.display=\"none\"' "
     s += "onchange='document.all.tmpSelectMonthLayer.style.display=\"none\";"
     s += "parent.nickTheMonth = this.value; parent.nickSetDay(parent.nickTheYear,parent.nickTheMonth)'>\r\n";
  var selectInnerHTML = s;
  for (var i = 1; i < 13; i++)
  {
    if (i == m)
       {selectInnerHTML += "<option  value='"+i+"' selected>"+i+"��"+"</option>\r\n";}
    else {selectInnerHTML += "<option  value='"+i+"'>"+i+"��"+"</option>\r\n";}
  }
  selectInnerHTML += "</select>";
  odatelayer.tmpSelectMonthLayer.style.display="";
  odatelayer.tmpSelectMonthLayer.innerHTML = selectInnerHTML;
  odatelayer.tmpSelectMonth.focus();
}

function closeLayer()               //�����Ĺر�
  {
    document.all.DateTimePicker_DateLayer.style.display="none";
  }

function IsPinYear(year)            //�ж��Ƿ���ƽ��
  {
    if (0==year%4&&((year%100!=0)||(year%400==0))) return true;else return false;
  }

function GetMonthCount(year,month)  //�������Ϊ29��
  {
    var c=MonHead[month-1];if((month==2)&&IsPinYear(year)) c++;return c;
  }
function GetDOW(day,month,year)     //��ĳ������ڼ�
  {
    var dt=new Date(year,month-1,day).getDay()/7; return dt;
  }

function nickPrevY()  //��ǰ�� Year
  {
    if(nickTheYear > 999 && nickTheYear <10000){nickTheYear--;}
    else{alert("��ݳ�����Χ��1000-9999����");}
    nickSetDay(nickTheYear,nickTheMonth);
  }
function nickNextY()  //���� Year
  {
    if(nickTheYear > 999 && nickTheYear <10000){nickTheYear++;}
    else{alert("��ݳ�����Χ��1000-9999����");}
    nickSetDay(nickTheYear,nickTheMonth);
  }
function nickToday()  //Today Button
  {
 var today;
    nickTheYear = new Date().getFullYear();
    nickTheMonth = new Date().getMonth()+1;
     if (nickTheMonth < 10){nickTheMonth = "0" + nickTheMonth;}
    today=new Date().getDate();
     if (today < 10){today = "0" + today;}
    //nickSetDay(nickTheYear,nickTheMonth);
    if(outObject){
  outObject.value=nickTheYear + "-" + nickTheMonth + "-" + today;
    }
    closeLayer();
  }
function nickPrevM()  //��ǰ���·�
  {
    if(nickTheMonth>1){nickTheMonth--}else{nickTheYear--;nickTheMonth=12;}
    nickSetDay(nickTheYear,nickTheMonth);
  }
function nickNextM()  //�����·�
  {
    if(nickTheMonth==12){nickTheYear++;nickTheMonth=1}else{nickTheMonth++}
    nickSetDay(nickTheYear,nickTheMonth);
  }

function nickSetDay(yy,mm)   //��Ҫ��д����**********
{
  nickWriteHead(yy,mm);
  //���õ�ǰ���µĹ�������Ϊ����ֵ
  nickTheYear=yy;
  nickTheMonth=mm;

  for (var i = 0; i < 39; i++){nickWDay[i]=""};  //����ʾ�������ȫ�����
  var day1 = 1,day2=1,firstday = new Date(yy,mm-1,1).getDay();  //ĳ�µ�һ������ڼ�
  for (i=0;i<firstday;i++)nickWDay[i]=GetMonthCount(mm==1?yy-1:yy,mm==1?12:mm-1)-firstday+i+1 //�ϸ��µ������
  for (i = firstday; day1 < GetMonthCount(yy,mm)+1; i++){nickWDay[i]=day1;day1++;}
  for (i=firstday+GetMonthCount(yy,mm);i<39;i++){nickWDay[i]=day2;day2++}
  for (i = 0; i < 39; i++)
  { var da = eval("odatelayer.nickDay"+i)     //��д�µ�һ���µ�������������
    if (nickWDay[i]!="")
      {
  //��ʼ���߿�
  da.borderColorLight="#9496E1";
  da.borderColorDark="#FFFFFF";
  if(i<firstday)  //�ϸ��µĲ���
  {
   da.innerHTML="<b><font color=gray>" + nickWDay[i] + "</font></b>";
   da.title=(mm==1?12:mm-1) +"��" + nickWDay[i] + "��";
   da.onclick=Function("nickDayClick(this.innerText,-1)");
   if(!outDate)
    da.style.backgroundColor = ((mm==1?yy-1:yy) == new Date().getFullYear() &&
     (mm==1?12:mm-1) == new Date().getMonth()+1 && nickWDay[i] == new Date().getDate()) ?
      "#C6C7EF":"#E0E0E0";
   else
   {
    da.style.backgroundColor =((mm==1?yy-1:yy)==outDate.getFullYear() && (mm==1?12:mm-1)== outDate.getMonth() + 1 &&
    nickWDay[i]==outDate.getDate())? "#FFD700" :
    (((mm==1?yy-1:yy) == new Date().getFullYear() && (mm==1?12:mm-1) == new Date().getMonth()+1 &&
    nickWDay[i] == new Date().getDate()) ? "#C6C7EF":"#E0E0E0");
    //��ѡ�е�������ʾΪ����ȥ
    if((mm==1?yy-1:yy)==outDate.getFullYear() && (mm==1?12:mm-1)== outDate.getMonth() + 1 &&
    nickWDay[i]==outDate.getDate())
    {
     da.borderColorLight="#FFFFFF";
     da.borderColorDark="#9496E1";
    }
   }
  }
  else if (i>=firstday+GetMonthCount(yy,mm))  //�¸��µĲ���
  {
   da.innerHTML="<b><font color=gray>" + nickWDay[i] + "</font></b>";
   da.title=(mm==12?1:mm+1) +"��" + nickWDay[i] + "��";
   da.onclick=Function("nickDayClick(this.innerText,1)");
   if(!outDate)
    da.style.backgroundColor = ((mm==12?yy+1:yy) == new Date().getFullYear() &&
     (mm==12?1:mm+1) == new Date().getMonth()+1 && nickWDay[i] == new Date().getDate()) ?
      "#C6C7EF":"#E0E0E0";
   else
   {
    da.style.backgroundColor =((mm==12?yy+1:yy)==outDate.getFullYear() && (mm==12?1:mm+1)== outDate.getMonth() + 1 &&
    nickWDay[i]==outDate.getDate())? "#FFD700" :
    (((mm==12?yy+1:yy) == new Date().getFullYear() && (mm==12?1:mm+1) == new Date().getMonth()+1 &&
    nickWDay[i] == new Date().getDate()) ? "#C6C7EF":"#E0E0E0");
    //��ѡ�е�������ʾΪ����ȥ
    if((mm==12?yy+1:yy)==outDate.getFullYear() && (mm==12?1:mm+1)== outDate.getMonth() + 1 &&
    nickWDay[i]==outDate.getDate())
    {
     da.borderColorLight="#FFFFFF";
     da.borderColorDark="#9496E1";
    }
   }
  }
  else  //���µĲ���
  {
   da.innerHTML="<b>" + nickWDay[i] + "</b>";
   da.title=mm +"��" + nickWDay[i] + "��";
   da.onclick=Function("nickDayClick(this.innerText,0)");  //��td����onclick�¼��Ĵ���
   //����ǵ�ǰѡ������ڣ�����ʾ����ɫ�ı���������ǵ�ǰ���ڣ�����ʾ����ɫ�ı���
   if(!outDate)
    da.style.backgroundColor = (yy == new Date().getFullYear() && mm == new Date().getMonth()+1 && nickWDay[i] == new Date().getDate())?
     "#C6C7EF":"#E0E0E0";
   else
   {
    da.style.backgroundColor =(yy==outDate.getFullYear() && mm== outDate.getMonth() + 1 && nickWDay[i]==outDate.getDate())?
     "#FFD700":((yy == new Date().getFullYear() && mm == new Date().getMonth()+1 && nickWDay[i] == new Date().getDate())?
     "#C6C7EF":"#E0E0E0");
    //��ѡ�е�������ʾΪ����ȥ
    if(yy==outDate.getFullYear() && mm== outDate.getMonth() + 1 && nickWDay[i]==outDate.getDate())
    {
     da.borderColorLight="#FFFFFF";
     da.borderColorDark="#9496E1";
    }
   }
  }
        da.style.cursor="hand"
      }
    else{da.innerHTML="";da.style.backgroundColor="";da.style.cursor="default"}
  }
}

function nickDayClick(n,ex)  //�����ʾ��ѡȡ���ڣ������뺯��*************
{
  var yy=nickTheYear;
  var mm = parseInt(nickTheMonth)+ex; //ex��ʾƫ����������ѡ���ϸ��·ݺ��¸��·ݵ�����
 //�ж��·ݣ������ж�Ӧ�Ĵ���
 if(mm<1){
  yy--;
  mm=12+mm;
 }
 else if(mm>12){
  yy++;
  mm=mm-12;
 }

  if (mm < 10){mm = "0" + mm;}
  if (outObject)
  {
    if (!n) {//outObject.value="";
      return;}
    if ( n < 10){n = "0" + n;}
    outObject.value= yy + "-" + mm + "-" + n ; //ע�����������������ĳ�����Ҫ�ĸ�ʽ
    closeLayer();
  }
  else {closeLayer(); alert("����Ҫ����Ŀؼ����󲢲����ڣ�");}
}
