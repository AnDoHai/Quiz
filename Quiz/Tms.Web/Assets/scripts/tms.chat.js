
var orderChatModule = {
    //hostUrl:"http://admin.nhonhoa.vn",
    init: function () {
        $(function () {
            var chatElem = $('.chat-box .panel-body');
            chatElem.scrollTop(chatElem.prop("scrollHeight"));
            //Set the hubs URL for the connection
            $.connection.hub.url = "/signalr";

            // Declare a proxy to reference the hub.
            var chat = $.connection.notificationHub;

            // Create a function that the hub can call to broadcast messages.
            chat.client.addMessage = function (userName, message, orderId, userId, isAdmin) {
                orderChat.fillMessageContent(userName, message, orderId, userId, isAdmin);
            };

            $.connection.hub.start().done(function () {
                $('#btn-chat').click(function () {
                    var orderId = $('.chat').attr('data-order-id');
                    var userId = $('.chat').attr('data-user-id');
                    var message = $('#btn-input').val();
                    var userName = $(".chat").attr("data-userName");
                    var isAdmin = 1;
                    orderChat.storeMessage(orderId, userId, message, function () {
                        chat.server.addMessage(userName, message, orderId, userId, isAdmin);
                        // Clear text box and reset focus for next comment.
                        $('#btn-input').val('').focus();
                    })
                    // Call the Send method on the hub.

                });
                $('#btn-input').keyup(function (e) {
                    if (e.keyCode == 13) {
                        $('#btn-chat').click();
                    }
                });

            });
        });
    }
}
