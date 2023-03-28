namespace Notes.Test;
using Notes.Shared;

public class NotesUnitTests
{
    [Fact]
    public void TestNoteEntity()
    {
        //arrange
        DateTime expectedDate = new DateTime(2023,3,28);
        Note testNote = new();

        //act
        testNote.NoteTitle = "First day at work";
        testNote.NoteBody = "there are alot of things I need to do...";
        testNote.NoteDate = expectedDate;

        //Assert
        Assert.Equal("First day at work", testNote.NoteTitle);
        Assert.Equal("there are alot of things I need to do...", testNote.NoteBody);
        Assert.Equal(expectedDate, testNote.NoteDate);



    }
}