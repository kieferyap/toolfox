// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function deletePlace(placeId)
{
  $.ajax({
    url: '/Place/Delete',
    type: 'POST',
    data: {
      id: placeId
    }
  }).then(function(data) {
    window.location = '/Place';
  })
}

// Flashcards
const FLASHCARD_LABEL_INVISIBLE_CLASS = 'image-only'
var flashcardImageFile = null;
var flashcardDataUrlFile = null;
var flashcardLabel = null;
var flashcardFont = 'lined';
var flashcardType = 'image-text';
var flashcardNotes = '';
var flashcardOrientation = 'portrait';

// Preview the image of the flashcard
function flashcardImageUpdate()
{
  flashcardImageFile = $('#flashcard-image').prop('files');
  if (flashcardImageFile.length) {
    flashcardDataUrlFile = window.URL.createObjectURL(flashcardImageFile[0]);
  }

  if (flashcardDataUrlFile != null) {
    $('#preview-flashcard-image').attr('class', 'd-block justify-content-center');
    // $('#flashcard-image-data-url').attr('value', flashcardDataUrlFile);
    $('#preview-flashcard-image-none').attr('class', 'd-none');
    $('#preview-flashcard-image-sm').attr('src', flashcardDataUrlFile);
    $('#preview-flashcard-image-md').attr('src', flashcardDataUrlFile);
  } else {
    $('#preview-flashcard-image').attr('class', 'd-none');
    $('#preview-flashcard-image-none').attr('class', 'text-secondary');
  }
}

// Preview the label of the flashcard
function flashcardLabelUpdate()
{
  flashcardLabel = $('#flashcard-label').val();
  if (flashcardLabel) {
    $('#preview-flashcard-label').html(flashcardLabel);
  } else {
    $('#preview-flashcard-label').html('Untitled');
  }
}

// Update the font of the flashcard
function flashcardFontUpdate()
{
  flashcardFont = $('#flashcard-font').val();
  $('#preview-flashcard-label').attr('class', `flashcard-label-${flashcardFont}`);
}

// Update the flashcard type
function flashcardTypeUpdate()
{
  flashcardType = $('#flashcard-type').val();
  if (flashcardType === FLASHCARD_LABEL_INVISIBLE_CLASS) {
    $('#preview-flashcard-label-parent').css('display', 'none');
  } else {
    $('#preview-flashcard-label-parent').css('display', 'block');
  }
}

function disableButtonSeconds(seconds, buttonSelector, formSelector)
{
  var buttonOriginalText = $(buttonSelector).val();
  $(buttonSelector).attr('disabled', true);
  $(buttonSelector).val('Loading...');
  $(formSelector)[0].submit();
  setTimeout(function (){
    $(buttonSelector).val(buttonOriginalText);
    $(buttonSelector).attr('disabled', false);
  }, seconds * 1000); 
}
