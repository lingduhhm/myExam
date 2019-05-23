/**
 * Created by user on 2018/7/19.
 */
/**
 * Created by user on 2018/7/18.
 */
var usersBoxUserData = ["User1",
    "User2",
    "User3",
    "User4",
    "User5",
    "User6",
    "User7",
    "User8",
    "User9",
    "User10",
    "User11"];
var usersBoxPositionData = ["Manager",
    "Employee",
    "Employee",
    "Employee",
    "Employee",
    "Employee",
    "Employee",
    "Employee",
    "Employee",
    "Employee",
    "Employee"];
for (var i = 0; i < usersBoxUserData.length; i++) {
    $(".usersCon1 .usersBox div").append("<input type='radio' name='user'><span id='usersBoxUser'>" + usersBoxUserData[i] + "</span>&nbsp<span id='usersBoxPosition'>" + usersBoxPositionData[i] + "</span><br>");
}
$(".usersCon1 .usersBox .edit").on("click",function(){
    $(".Users .resetPassword h1").text($(".usersCon1 .usersBox div input:checked").next().text());
});
