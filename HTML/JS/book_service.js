$('[data-tab]').on('click', function () {
    $(this).addClass('active').siblings('[data-tab]').removeClass('active')
    $(this).siblings('[data-content=' + $(this).data('nav-item') + ']').addClass('active').siblings('[data-content]').removeClass('active')
  })
 
  function openTab(evt, tabName) {
   var i, tabcontent, tablinks;
   tabcontent = document.getElementsByClassName("tabcontent");
   for (i = 0; i < tabcontent.length; i++) {
     tabcontent[i].style.display = "none";
   }
   tablinks = document.getElementsByClassName("nav-item");
   for (i = 0; i < tablinks.length; i++) {
     tablinks[i].className = tablinks[i].className.replace(" active", "");
   }
   document.getElementById(tabName).style.display = "block";
   evt.currentTarget.className += " active";
 }
 
 // Get the element with id="defaultOpen" and click on it
 document.getElementById("defaultOpen").click();
 
 // $(document).on('click', '.nav-item', function () {
 //   $('.tab-1').removeClass('active');
 //   $(this).children(".nav-link").children(".menu-title").addClass('active-tab');
 //   //$(this).addClass('active-tab').siblings().removeClass('active-tab')
 // })
 
 var address;
 function show_form(){
   if(address==1)
   {
     document.getElementById("address_form").style.display="block";
     return address=0;
   }else{
     document.getElementById("address_form").style.display="none";
     return address=1;
   }
   
 }
 function cancel_form(){
   if(address==1)
   {
     document.getElementById("address_form").style.display="inline";
     return address=0;
   }else{
     document.getElementById("address_form").style.display="none";
     return address=1;
   }
 }












 