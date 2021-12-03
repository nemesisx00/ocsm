import './EntryList.css'
import React from 'react'

export default class EntryList extends React.Component
{
	render()
	{
		let entries = this.generateEntries()
		
		return (
			<div className="entryList">
				<div className="entryListLabel">{this.props.title}</div>
				{entries}
				<button className="newEntryButton" onClick={() => this.addNewEntry()}>{this.props.buttonLabel}</button>
			</div>
		)
	}
	
	generateEntries()
	{
		let entries = []
		
		this.props.values.forEach((value, i) => {
			entries.push(<input key={`entry-${i}`} className="entry" type="text" value={value} onChange={(e) => this.props.entryChangeHandler(i, e.target.value)} />)
		})
		
		return entries
	}
	
	addNewEntry()
	{
		this.props.entryChangeHandler(null, '')
	}
}
