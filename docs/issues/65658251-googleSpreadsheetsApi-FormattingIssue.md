# Issue related to limiting formatting after reaching certain amount of data

Link to StackOverflow question: [Why Google spreadsheets API stops applying formatting after entering certain amount of data?](https://stackoverflow.com/questions/65658251/why-google-spreadsheets-api-stops-applying-formatting-after-entering-certain-amo)

Sample Google spreadsheet with full priviledges: [https://docs.google.com/spreadsheets/d/1_EED0i0e_SjBzkXyjV_9OODcpOhDwk-WPo6lP2bpApQ/edit?usp=sharing](https://docs.google.com/spreadsheets/d/1_EED0i0e_SjBzkXyjV_9OODcpOhDwk-WPo6lP2bpApQ/edit?usp=sharing)

Sample test data:

- [JSON with removed "null" values](requestExample.json)
- [JSON with all values from request](requestExample_full.json)

Explanation of the batch request - what is it suppose to do ( [x] works, [ ] doesn't work):

- [x] Add date, format with bold
- [x] Merge date (usually there are multiple cells)
- [x] WrapStrategy, TextRotation, VerticalAlignment, HorizontalAlignment, Text-Bold, FontSize
- [x] TextRotation by angle (must be separate style)
- [x] 11x draw border
- [x] repeatCell - background color
- [ ] repeatCell - wrap strategy, verticalAlignment, horizontalAlignment
