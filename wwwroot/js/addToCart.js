function addToCart() {
    let cardElement = $(this).parents('.card').length != 0
        ? $(this).parents('.card')
        : $(this).parents('.hat-data');

    let newOrderItem = {
        id: Math.round(Math.random() * 1000),
        hatId: parseInt(cardElement.attr("id")),
        name: cardElement.find('.hat-name').text(),
        description: cardElement.find('.hat-description').val().trim(),
        image: cardElement.find('.hat-img').attr("src"),
        price: parseFloat(cardElement.find('.hat-price').text().replace("$", "").trim()),
        stock: parseInt(cardElement.find('.hat-stock').text()),
        quantity: parseInt(cardElement.find('.hat-quantity-input').val())
    }

    var index = -1;
    for (let i = 0; i < orderItems.length; i++) {
        if (orderItems[i].hatId == newOrderItem.hatId) {
            index = i;
            break;
        }
    }

    if (index == -1) {
        orderItems.push(newOrderItem);
        let cartEntry = createCartEntry(newOrderItem);
        $('#cart-dropdown-menu').append(cartEntry);
    }
    else {
        let item = orderItems[index];
        if (newOrderItem.quantity + item.quantity <= item.stock) {
            item.quantity = newOrderItem.quantity + item.quantity;
            let foundCartEntry = $('.cart-entry#' + item.id);
            foundCartEntry.find('.cart-hat-title').html(item.quantity + " x" + " " + item.name);
            foundCartEntry.find('.cart-hat-total').html("$" + item.quantity * item.price);
        }
        else {
            alert("Stock exceeded!");
            return 0;
        }
    }

    setCartTotal(orderItems);
    toggleCartButtons(1);

    localStorage.setItem("order", JSON.stringify(orderItems));
    $('.no-cart-items').html(orderItems.length);
}